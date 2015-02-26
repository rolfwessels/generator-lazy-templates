using System;
using System.Reflection;
using MainSolutionTemplate.Sdk.Helpers;
using Microsoft.Owin.Hosting;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	public class IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly static Lazy<string> _hostAddress;
		protected static Lazy<RestClient> _client;
		protected static Lazy<AuthResponse> _adminUser;
		public const string ClientId = "MainSolutionTemplate.Api";
		public const string AdminPassword = "admin!";
		public const string AdminUser = "admin";

		static IntegrationTestsBase()
		{
			_hostAddress = new Lazy<string>(StartHosting);
			_adminUser = new Lazy<AuthResponse>(LoggedInResponse);
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

		private static RestClient GetClient()
		{
			var client = new RestClient(_hostAddress.Value);
			return client;
		}

		protected RestRequest AdminRequest(string resource, Method method)
		{
			_log.Info("data");
			var request = new RestRequest(resource, method) {RequestFormat = DataFormat.Json};
			var loggedInResponse = _adminUser.Value;

			var format = string.Format("{0} {1}", loggedInResponse.token_type, loggedInResponse.access_token);
			_log.Info("Adding auth " + format);
			request.AddHeader("Authorization", format);
			return request;
		}

		private static AuthResponse LoggedInResponse()
		{
			var request = new RestRequest("Token", Method.POST);
			request.AddParameter("username", AdminUser);
			request.AddParameter("password", AdminPassword);
			request.AddParameter("grant_type", "password");
			request.AddParameter("client_id", ClientId);
			// action
			var loggedInResponse = _client.Value.ExecuteWithLogging<AuthResponse>(request);
			return loggedInResponse.Data;
		}

		#endregion

		protected class AuthResponse
		{
			public string access_token { get; set; }
			public string token_type { get; set; }
		}

	}
}