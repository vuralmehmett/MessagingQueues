namespace CommonQueueManager.Interface
{
    public interface IKafkaManager : IQueueManager
    {
        void Producer(string message);
        void Consumer();
    }
}
