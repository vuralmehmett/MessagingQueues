using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using KafkaNet;
using KafkaNet.Model;

namespace CommonQueueManager.ConnectionFactory
{
    public class KafkaConnectionFactory
    {
        private static readonly object SyncObj = new object();
        private static IBrokerRouter _brokerRouter;

        private static Dictionary<int, KafkaQueueConnection> _connectionsDict;

        public static Dictionary<int, KafkaQueueConnection> ConnectionsDict
        {
            get
            {
                if (_connectionsDict == null)
                {
                    _connectionsDict = new Dictionary<int, KafkaQueueConnection>();
                }

                return _connectionsDict;
            }
            set { }
        }

        public static IBrokerRouter CreateConnection(int threadId)
        {
            string uri = "http://" + ConfigurationManager.AppSettings["MessagingQueueHostAddress"] + ":" +
                        ConfigurationManager.AppSettings["KafkaPort"];

            lock (SyncObj)
            {
                var kafkaOptions = new KafkaOptions(new Uri(uri));
                var brokerRouter = new BrokerRouter(kafkaOptions);

                _brokerRouter = brokerRouter;

                if (ConnectionsDict.ContainsKey(threadId))
                {
                    var connection = ConnectionsDict.Where(x => x.Key == threadId).Select(c => c.Value.BrokerRouter).Single();
                    return connection;
                }

                ConnectionsDict.Add(threadId, new KafkaQueueConnection
                {
                    KafkaOptions = new KafkaOptions(),
                    BrokerRouter = _brokerRouter
                });

                return _brokerRouter;
            }
        }

        public static IBrokerRouter CreateRouter(int threadId, IBrokerRouter brokerRouter)
        {
            lock (SyncObj)
            {
                if (ConnectionsDict.ContainsKey(threadId) &&
                    ConnectionsDict[threadId].BrokerRouter != brokerRouter)
                {
                    throw new NotSupportedException();
                }

                _brokerRouter = brokerRouter;

                ConnectionsDict[threadId].BrokerRouter = _brokerRouter;

                return _brokerRouter;
            }
        }

        public static IBrokerRouter GetBrokerPerThreadId(int threadId)
        {
            if (!ConnectionsDict.ContainsKey(threadId))
            {
                throw new KeyNotFoundException();
            }

            return ConnectionsDict[threadId].BrokerRouter;
        }
    }

    public class KafkaQueueConnection
    {
        public KafkaOptions KafkaOptions { get; set; }
        public IBrokerRouter BrokerRouter { get; set; }
    }
}
