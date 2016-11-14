using System;
using System.Configuration;
using System.Text;
using CommonQueueManager.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommonQueueManager.QueueManager
{
    public class RabbitManager : IRabbitMqManager
    {
        private static readonly string TopicName = ConfigurationManager.AppSettings["QueueName"];
        public IConnection RabbitMqConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["MessagingQueueHostAddress"],
                UserName = ConfigurationManager.AppSettings["RabbitMQUserName"],
                Password = ConfigurationManager.AppSettings["RabbitMQPassword"],
                VirtualHost = ConfigurationManager.AppSettings["RabbitMQVirtualHost"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMQPort"])
            };

            var connection = factory.CreateConnection();
            return connection;
        }

        public IModel CreateChannel(IConnection conn)
        {
            var model = conn.CreateModel();

            return model;
        }

        private static void SetupInitialTopicQueue(IModel model)
        {
            model.QueueDeclare(TopicName, true, false, false, null);
        }

        public void SendMessage(string message, IModel model)
        {
            SetupInitialTopicQueue(model);
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish("", TopicName, basicProperties, messageBytes);
        }

        public void GetMessages(IModel model)
        {
            model.BasicQos(0, 1, false);

            var consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(TopicName, false, consumer);

            while (true)
            {
                //Get next message
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                //Serialize message
                var message = Encoding.Default.GetString(deliveryArgs.Body);

                Console.WriteLine("Message Received - {0}", message);
                model.BasicAck(deliveryArgs.DeliveryTag, false);
            }
        }
    }
}
