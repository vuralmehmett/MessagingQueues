using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using CommonQueueManager.ConnectionFactory;
using CommonQueueManager.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommonQueueManager.QueueManager
{
    public class RabbitManager : IRabbitMqManager
    {
        private static readonly string TopicName = ConfigurationManager.AppSettings["QueueName"];

        public RabbitManager()
        {
            var conn = RabbitMqConnectionFactory.CreateConnection(Thread.CurrentThread.ManagedThreadId);
            RabbitMqConnectionFactory.CreateChannel(Thread.CurrentThread.ManagedThreadId, conn);
        }

        public void SendMessage(string message)
        {
            var channel = RabbitMqConnectionFactory.GetChannelPerThreadId(Thread.CurrentThread.ManagedThreadId);

            channel.QueueDeclare(TopicName, true, false, false, null);

            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", TopicName, basicProperties, messageBytes);
        }

        public void GetMessage()
        {
            var channel = RabbitMqConnectionFactory.GetChannelPerThreadId(Thread.CurrentThread.ManagedThreadId);

            channel.BasicQos(0, 1, false);

            var consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(TopicName, false, consumer);

            while (true)
            {
                //Get next message
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                //Serialize message
                var message = Encoding.Default.GetString(deliveryArgs.Body);

                Console.WriteLine("Message Received from RabbitMQ - {0}", message);
                channel.BasicAck(deliveryArgs.DeliveryTag, false);
            }
        }

        public List<string> TestGet()
        {
            var messageList = new List<string>();

            var channel = RabbitMqConnectionFactory.GetChannelPerThreadId(Thread.CurrentThread.ManagedThreadId);

            var queueDeclareResponse = channel.QueueDeclare(TopicName, true, false, false, null);
            var consumer = new QueueingBasicConsumer(channel);

            try
            {
                channel.BasicConsume(TopicName, true, consumer);

                Console.WriteLine(" [*] Processing existing messages.");

                for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
                {
                    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    messageList.Add(message);
                    Console.WriteLine(" [x] Received {0}", message);
                }

            }
            catch (Exception)
            {
                var response = channel.BasicGet(TopicName, false);
                channel.BasicNack(response.DeliveryTag, true, true);
                throw;
            }

            return messageList;
        }
    }
}
