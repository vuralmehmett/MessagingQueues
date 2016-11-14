using RabbitMQ.Client;

namespace QueueManager.Interface
{
    public interface IRabbitMqManager
    {
        IConnection Connection();
        void PublishMessage();
        void SendMessage();
    }
}
