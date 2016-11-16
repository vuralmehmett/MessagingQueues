using System.Threading.Tasks;
using System.Web.Http;
using CommonQueueManager.Interface;
using MessagingQueues.WebApi.Model;

namespace MessagingQueues.WebApi.Controllers
{
    public class MessagingQueuesController : ApiController
    {
        private readonly IQueueManager _queueManager;
        public MessagingQueuesController(IQueueManager queueManager)
        {
            _queueManager = queueManager;
        }

        [HttpPost]
        public async Task<IHttpActionResult> SendData(MessagingQueue model)
        {
            _queueManager.SendMessage(model.Message);
            return await Task.FromResult<IHttpActionResult>(Ok());
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetData()
        {
            _queueManager.GetMessage();
            return await Task.FromResult<IHttpActionResult>(Ok());
        }
    }
}
