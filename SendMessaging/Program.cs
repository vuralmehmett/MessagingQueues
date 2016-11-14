using System;
using System.Configuration;
using CommonQueueManager.QueueManager;

namespace SendMessaging
{

    class Program
    {
        static void Main(string[] args)
        {
            if (Convert.ToInt16(ConfigurationManager.AppSettings["MessagingQueueOptions"]) == 0)
            {
                var rabbitManager = new RabbitManager();
                var connection = rabbitManager.RabbitMqConnection();

                var model = rabbitManager.CreateChannel(connection);

                Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");
                while (true)
                {
                    var message = Console.ReadLine();
                    if (message != null && message.ToLower() == "q") break;

                    rabbitManager.SendMessage(message, model);
                }

                rabbitManager.GetMessages(model);
            }

            else if (Convert.ToInt16(ConfigurationManager.AppSettings["MessagingQueueOptions"]) == 1)
            {
                var kafkaManager = new KafkaManager();

                Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");
                while (true)
                {
                    var message = Console.ReadLine();
                    if (message != null && message.ToLower() == "q") break;

                    kafkaManager.Producer(message);
                }

                kafkaManager.Consumer();
            }
        }
    }
}
