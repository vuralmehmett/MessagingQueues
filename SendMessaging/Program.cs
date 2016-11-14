using System;
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

            var ninjectConnect = Kernel.Get<IQueueManager>();

            Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");

            while (true)
            {
                var message = Console.ReadLine();
                if (message != null && message.ToLower() == "q") break;

                ninjectConnect.SendMessage(message);
            }

            ninjectConnect.GetMessage();
        }
    }
}
