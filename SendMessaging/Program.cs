using System;
using CommonQueueManager.Interface;
using CommonQueueManager.IoC;
using Ninject;
using RabbitMQ.Client.Framing.Impl;
using SendMessaging.Model;

namespace SendMessaging
{

    class Program
    {
        public static readonly StandardKernel Kernel = new StandardKernel();

        static void Main(string[] args)
        {
            var connection = new BaseHttpRequest.Connection();

            MessagingQueue messagingQueue = new MessagingQueue();

            for (int i = 0; i < 50; i++)
            {
                connection.Post(messagingQueue);
            }

            Console.WriteLine("bitti");

            //Kernel.Load(new QueueModule());

            //var ninjectConnect = Kernel.Get<IQueueManager>();

            //Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");

            ////while (true)
            ////{
            ////    var message = Console.ReadLine();
            ////    if (message != null && message.ToLower() == "q") break;

            ////    ninjectConnect.SendMessage(message);
            ////}

            //ninjectConnect.GetMessage();
        }
    }
}
