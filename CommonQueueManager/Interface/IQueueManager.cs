using System.Collections.Generic;

namespace CommonQueueManager.Interface
{
    public interface IQueueManager
    {
        void SendMessage(string message);
        void GetMessage();
        //List<string> TestGet();
    }
}
