using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AzureServiceConnect.Controllers;
using Microsoft.ServiceBus.Messaging;

namespace AzureServiceConnect
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            const string eventHubConnectionString = "Endpoint=sb://eventhubazconnect.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LTP6l6qeRXIIfJjIZcDl+a0Sl5urmiTpFGD0ByR+4Ek=";
            const string eventHubName = "insights-operational-logs";
            const string storageAccountName = "storageaccountazconnect";
            const string storageAccountKey = "/+oRjv+2Eco5/DvyWVYiXy+WF6I+L4fMSKAbbyTvKPrwq7Jv1YFEO5RdaRc7KOzlK9+8JrTznsbB1zoJUC978A==";
            var storageConnectionString =
                $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";

            var eventProcessorHostName = Guid.NewGuid().ToString();
            var eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName,
                EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);

            Debug.WriteLine("Registering EventProcessor...");
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();

            Debug.WriteLine("Receiving. Press enter key to stop worker.");
            //Console.ReadLine();
            //eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
