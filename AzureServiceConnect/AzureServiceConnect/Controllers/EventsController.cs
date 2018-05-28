using System.Web.Http;
using AzureServiceConnect.Models;

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
