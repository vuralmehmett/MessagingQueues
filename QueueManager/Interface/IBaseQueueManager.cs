using System.Collections.Generic;

namespace QueueManager.Interface
{
    interface IBaseQueueManager
    {
        List<string> GetAllMessage();
        List<string> GetSpesificMessage();
    }
}
