using System.Web.Http;

namespace AzureServiceConnect.Controllers
{
    public class EventsController : ApiController
    {
        public Records[] Get()
        {
            return AzureEventAggregator.GetLoggedEvents();
        }
    }
}
