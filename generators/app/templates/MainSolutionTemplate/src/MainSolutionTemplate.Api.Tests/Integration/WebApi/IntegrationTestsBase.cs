﻿using System;
using System.Reflection;
using System.Threading;
using MainSolutionTemplate.Core.Helpers;
using Microsoft.Owin.Hosting;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Api.Tests.Integration.WebApi
{
	public class IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly static Lazy<string> _hostAddress;
		protected static Lazy<RestClient> _client;
		private static Lazy<AuthResponse> _adminUser;

		static IntegrationTestsBase()
		{
			_hostAddress = new Lazy<string>(StartHosting);
			_adminUser = new Lazy<AuthResponse>(LoggedInResponse);
			_client = new Lazy<RestClient>(GetClient);
		}

		public IntegrationTestsBase()
		{
			
			
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
			request.AddParameter("username", "admin");
			request.AddParameter("password", "admin!");
			request.AddParameter("grant_type", "password");
			request.AddParameter("client_id", "MainSolutionTemplateApi");
			// action
			var loggedInResponse = _client.Value.ExecuteWithLogging<AuthResponse>(request);
			return loggedInResponse.Data;
		}

		#endregion


		private class AuthResponse
		{
			public string access_token { get; set; }
			public string token_type { get; set; }
		}

	}
}