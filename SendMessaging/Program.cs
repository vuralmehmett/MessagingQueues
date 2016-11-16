using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            const int NO_OF_TASKS = 20;

            List<Task> listOfTask = new List<Task>();

            for (int i = 0; i < NO_OF_TASKS; ++i)
            {
                int taskId = i;

                Task tsk = new Task(() =>
                {
                    SendMessageToQueue.Start(taskId);

                }, TaskCreationOptions.LongRunning);

                listOfTask.Add(tsk);
            }

            listOfTask.ForEach(t => t.Start());

            Task.WaitAll(listOfTask.ToArray());





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

        class SendMessageToQueue
        {

            public static void SendMessage(int taskId)
            {
                var connection = new BaseHttpRequest.Connection();

                MessagingQueue messagingQueue = new MessagingQueue();

                connection.Post(messagingQueue);

                Console.WriteLine("Generating a sample for task id {0}", taskId);
            }

            public static void Start(int taskId)
            {
                while (true)
                {
                    SendMessage(taskId);

                    //Thread.Sleep(5);
                }
            }
        }
    }
}
