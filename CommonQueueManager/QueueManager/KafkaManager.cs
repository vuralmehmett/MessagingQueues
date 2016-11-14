using System;
using System.Configuration;
using System.Text;
using CommonQueueManager.Interface;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace CommonQueueManager.QueueManager
{
    public class KafkaManager : IKafkaManager
    {
        private readonly IBrokerRouter _brokerRouter;

        public KafkaManager()
        {
            string uri = "http://" + ConfigurationManager.AppSettings["MessagingQueueHostAddress"] + ":" +
                        ConfigurationManager.AppSettings["KafkaPort"];

            var kafkaOptions = new KafkaOptions(new Uri(uri));
            var brokerRouter = new BrokerRouter(kafkaOptions);

            _brokerRouter = brokerRouter;
        }

        public void Producer(string message)
        {
            var topic = ConfigurationManager.AppSettings["QueueName"];

            var producer = new Producer(_brokerRouter);

            producer.SendMessageAsync(topic, new[] { new Message(message), }).Wait();
        }

        public void Consumer()
        {
            var topic = ConfigurationManager.AppSettings["QueueName"];

            Console.WriteLine("Consume :" + topic);
            Console.WriteLine("\n");
            Console.WriteLine($" Consuming {topic}");

            var consumer = new Consumer(new ConsumerOptions(topic, _brokerRouter));

            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Response : PartitionId:{0}, Offset:{1}, Message:{2}",
                    message.Meta.PartitionId, message.Meta.Offset, Encoding.UTF8.GetString(message.Value));
            }
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void GetMessage()
        {
            throw new NotImplementedException();
        }
    }
}
