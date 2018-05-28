using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureServiceConnect.Models
{
	public class Records
	{
		public DateTime Time;
		public string ResourceId;
		public string OperationName;
		public string Category;
		public string ResultType;
		public string ResultSignature;
		public string DurationMs;
		public string CallerIpAddress;
		public string CorrelationId;
		public string Level;
		public string Location;
	}
}