using System;
using System.Configuration;

namespace CommonQueueManager
{
    public class Connector
    {
        public RabbitMQ.Client.ConnectionFactory RabbitMqConnection()
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["MessagingQueueHostAddress"],
                UserName = ConfigurationManager.AppSettings["RabbitMQUserName"],
                Password = ConfigurationManager.AppSettings["RabbitMQPassword"],
                VirtualHost = ConfigurationManager.AppSettings["RabbitMQVirtualHost"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMQPort"])
            };

            return factory;
        }
    }
}
