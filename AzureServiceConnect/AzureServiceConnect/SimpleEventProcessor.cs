using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;
using System.Linq;
using AzureServiceConnect.Models;
using Newtonsoft.Json;

namespace AzureServiceConnect
{
	public class SimpleEventProcessor : IEventProcessor
	{
		Stopwatch _checkpointStopWatch;

		async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
		{
			Debug.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason);
			if (reason == CloseReason.Shutdown)
			{
				await context.CheckpointAsync();
			}
		}

		Task IEventProcessor.OpenAsync(PartitionContext context)
		{
			Debug.WriteLine("SimpleEventProcessor initialized.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset);
			this._checkpointStopWatch = new Stopwatch();
			this._checkpointStopWatch.Start();
			return Task.FromResult<object>(null);
		}

		public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
		{
			Debug.WriteLine("triggered" + messages);

			foreach (EventData eventData in messages)
			{
				var data = Encoding.UTF8.GetString(eventData.GetBytes());

				Debug.WriteLine("Message received.  Partition: '{0}', Data: '{1}'", context.Lease.PartitionId, data);

				var e = JsonConvert.DeserializeObject<AzureEvent>(data);

				AzureEventAggregator.LogEvent(e.Records);
			}

			if (_checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
			{
				//MessagingEntityDisabledException Check point to preserve logs
				//await context.CheckpointAsync();
				//this._checkpointStopWatch.Restart();
			}
		}

	}

	public class AzureEventAggregator
	{
		private static readonly List<Records> Events = new List<Records>();

		public static void LogEvent(List<Records> records)
		{
			if (Events.Count < 1024)
			{
				foreach (var record in records)
				{

					record.Time = Convert.ToDateTime(record.Time.ToLongDateString());
					Events.Add(record);
				}
			}
		}

		public static Records[] GetLoggedEvents()
		{
			var events = Events.ToArray().OrderBy(x => x.Time).ToArray();
			return events;
		}
	}



}