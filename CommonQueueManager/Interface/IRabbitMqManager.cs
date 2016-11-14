using System.Collections.Generic;
using CommonQueueManager.QueueManager;
using RabbitMQ.Client;

namespace CommonQueueManager.Interface
{
    public interface IRabbitMqManager : IQueueManager
    {
        IConnection RabbitMqConnection();
        IModel CreateChannel(IConnection conn);
        void SendMessage(string message, IModel model);
        void GetMessages(IModel model);
    }
}
