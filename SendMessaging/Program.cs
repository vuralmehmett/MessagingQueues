using System;
using System.Configuration;
using CommonQueueManager.Interface;
using CommonQueueManager.IoC;
using Ninject;

namespace SendMessaging
{

    class Program
    {
        public static readonly StandardKernel Kernel = new StandardKernel();

        static void Main(string[] args)
        {
            Kernel.Load(new QueueModule());

            if (Convert.ToInt16(ConfigurationManager.AppSettings["MessagingQueueOptions"]) == 0)
            {
                var ninjectRabbitMqConnect = Kernel.Get<IQueueManager>();

                Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");
                while (true)
                {
                    var message = Console.ReadLine();
                    if (message != null && message.ToLower() == "q") break;

                    ninjectRabbitMqConnect.SendMessage(message);
                }

                ninjectRabbitMqConnect.GetMessage();
            }

            else if (Convert.ToInt16(ConfigurationManager.AppSettings["MessagingQueueOptions"]) == 1)
            {

                var ninjectKafkaConnect = Kernel.Get<IKafkaManager>();

                Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");
                while (true)
                {
                    var message = Console.ReadLine();
                    if (message != null && message.ToLower() == "q") break;

                    ninjectKafkaConnect.Producer(message);
                }

                ninjectKafkaConnect.Consumer();
            }
        }
    }
}
