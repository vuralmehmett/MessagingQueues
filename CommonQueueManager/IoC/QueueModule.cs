using CommonQueueManager.Interface;
using CommonQueueManager.QueueManager;
using KafkaNet;
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
            //Bind<IRabbitMqManager>().To<RabbitManager>();
            //Bind<IKafkaManager>().To<KafkaManager>();
            Bind<IQueueManager>().To<RabbitManager>();
        }
    }
}
