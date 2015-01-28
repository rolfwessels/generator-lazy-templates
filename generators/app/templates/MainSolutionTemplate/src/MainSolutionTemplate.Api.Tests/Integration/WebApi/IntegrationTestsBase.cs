using System;
using System.Reflection;
using System.Threading;
using Microsoft.Owin.Hosting;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Api.Tests.Integration.WebApi
{
	public class IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly static Lazy<string> _hostAddress;
		protected Lazy<RestClient> _client;

		static IntegrationTestsBase()
		{
			_hostAddress = new Lazy<string>(StartHosting);
		}

		public IntegrationTestsBase()
		{
			
			_client = new Lazy<RestClient>(GetClient);
		}

		#region Private Methods

		private static string StartHosting()
		{
			int port = new Random().Next(9000, 9999);
			string address = string.Format("http://localhost:{0}/", port);
			_log.Info(string.Format("Starting api on [{0}]", address));
			WebApp.Start<Api.Startup>(address);
			return address;
		}

		private RestClient GetClient()
		{
			var client = new RestClient(_hostAddress.Value);
			return client;
		}

		#endregion

	}
}