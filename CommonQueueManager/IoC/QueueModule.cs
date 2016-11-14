using System;
using System.Configuration;
using CommonQueueManager.Interface;
using CommonQueueManager.QueueManager;
using Ninject.Modules;

namespace CommonQueueManager.IoC
{
    public class QueueModule : NinjectModule
    {
        /// <summary>
        /// this module is use Ninject
        /// </summary>
        public override void Load()
        {
            if (Convert.ToInt16(ConfigurationManager.AppSettings["MessagingQueueOptions"]) == 0)
            {
                Bind<IQueueManager>().To<RabbitManager>();
            }

            else if (Convert.ToInt16(ConfigurationManager.AppSettings["MessagingQueueOptions"]) == 1)
            {
                Bind<IQueueManager>().To<KafkaManager>();
            }
        }
    }
}
