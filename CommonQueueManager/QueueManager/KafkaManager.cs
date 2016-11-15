using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using CommonQueueManager.ConnectionFactory;
using CommonQueueManager.Interface;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace CommonQueueManager.QueueManager
{
    public class KafkaManager : IKafkaManager
    {
        public KafkaManager()
        {
            var conn = KafkaConnectionFactory.CreateConnection(Thread.CurrentThread.ManagedThreadId);
            KafkaConnectionFactory.CreateRouter(Thread.CurrentThread.ManagedThreadId, conn);
        }

        public void SendMessage(string message)
        {
            var topic = ConfigurationManager.AppSettings["QueueName"];

            var brokerRouter = KafkaConnectionFactory.GetBrokerPerThreadId(Thread.CurrentThread.ManagedThreadId);

            var producer = new Producer(brokerRouter);

            producer.SendMessageAsync(topic, new[] { new Message(message), }).Wait();

        }

        public void GetMessage()
        {
            var topic = ConfigurationManager.AppSettings["QueueName"];

            var brokerRouter = KafkaConnectionFactory.GetBrokerPerThreadId(Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("Consume from Kafka :" + topic);
            Console.WriteLine("\n");
            Console.WriteLine($" Consuming from Kafka {topic}");

            var consumer = new Consumer(new ConsumerOptions(topic, brokerRouter));

            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Response : PartitionId:{0}, Offset:{1}, Message:{2}",
                    message.Meta.PartitionId, message.Meta.Offset, Encoding.UTF8.GetString(message.Value));
            }
        }

        //public List<KafkaMessaging> TestGet()
        //{
        //    List<KafkaMessaging> kafkaMessagingList = new List<KafkaMessaging>();

        //    var topic = ConfigurationManager.AppSettings["QueueName"];

        //    var brokerRouter = KafkaConnectionFactory.GetBrokerPerThreadId(Thread.CurrentThread.ManagedThreadId);

        //    var consumer = new Consumer(new ConsumerOptions(topic, brokerRouter));

        //    foreach (var message in consumer.Consume())
        //    {
        //        KafkaMessaging messaging = new KafkaMessaging
        //        {
        //            PartitionId = message.Meta.PartitionId,
        //            Offset = (int) message.Meta.Offset,
        //            Message = Encoding.UTF8.GetString(message.Value)
        //        };

        //        kafkaMessagingList.Add(messaging);
        //        //Console.WriteLine("Response : PartitionId:{0}, Offset:{1}, Message:{2}",
        //        //    message.Meta.PartitionId, message.Meta.Offset, Encoding.UTF8.GetString(message.Value));
        //    }

        //    return kafkaMessagingList;
        //}

        //List<string> IQueueManager.TestGet()
        //{
        //    List<KafkaMessaging> kafkaMessagingList = new List<KafkaMessaging>();

        //    var topic = ConfigurationManager.AppSettings["QueueName"];

        //    var brokerRouter = KafkaConnectionFactory.GetBrokerPerThreadId(Thread.CurrentThread.ManagedThreadId);

        //    var consumer = new Consumer(new ConsumerOptions(topic, brokerRouter));

        //    var asd = consumer.GetTopic(topic);
        //    var sss = consumer.ConsumerTaskCount;
        //    var ssss = consumer.GetOffsetPosition();


        //    foreach (var message in consumer.Consume())
        //    {
        //        KafkaMessaging messaging = new KafkaMessaging
        //        {
        //            PartitionId = message.Meta.PartitionId,
        //            Offset = (int)message.Meta.Offset,
        //            Message = Encoding.UTF8.GetString(message.Value)
        //        };

        //        kafkaMessagingList.Add(messaging);
        //        //Console.WriteLine("Response : PartitionId:{0}, Offset:{1}, Message:{2}",
        //        //    message.Meta.PartitionId, message.Meta.Offset, Encoding.UTF8.GetString(message.Value));
        //    }

        //    var ss = kafkaMessagingList;

        //    return new List<string>();
        }

        //public class KafkaMessaging
        //{
        //    public int PartitionId { get; set; }
        //    public int Offset { get; set; }
        //    public string Message { get; set; }
        //}
    }

