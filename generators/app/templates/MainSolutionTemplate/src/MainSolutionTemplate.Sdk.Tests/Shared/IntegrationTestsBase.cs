using System;
using System.IO;
using System.Reflection;
using MainSolutionTemplate.Api.AppStartup;
using NCrunch.Framework;
using log4net;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Shared.Models;
using Microsoft.Owin.Hosting;
using RestSharp;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
	public class IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly static Lazy<string> _hostAddress;

        protected static Lazy<TokenResponseModel> _adminUser;
	    protected static Lazy<RestConnectionFactory> _defaultRequestFactory;
	    protected static Lazy<RestConnectionFactory> _adminRequestFactory;
	    public const string ClientId = "MainSolutionTemplate.Api";
		public const string AdminPassword = "admin!";
		public const string AdminUser = "admin";

		static IntegrationTestsBase()
		{
            
		    _hostAddress = new Lazy<string>(StartHosting);
            _adminUser = new Lazy<TokenResponseModel>(LoggedInResponse);
            _defaultRequestFactory = new Lazy<RestConnectionFactory>(() => new RestConnectionFactory(_hostAddress.Value));
            _adminRequestFactory = new Lazy<RestConnectionFactory>(() => new RestConnectionFactory(_hostAddress.Value));
            
		}

	    public string SignalRUri {
	        get {
                return _defaultRequestFactory.Value.GetClient().BuildUri(new RestRequest("signalr")).ToString();
            }
	    }

		#region Private Methods

		private static string StartHosting()
		{
			int port = new Random().Next(9000, 9999);
			string address = string.Format("http://localhost:{0}/", port);
			_log.Info(string.Format("Starting api on [{0}]", address));
            Console.Out.WriteLine("NCrunchEnvironment.GetOriginalSolutionPath():" + NCrunchEnvironment.GetOriginalSolutionPath());
            SimpleFileServer.PossibleWebBasePath = Path.Combine(Path.GetDirectoryName( NCrunchEnvironment.GetOriginalSolutionPath()),"MainSolutionTemplate.Website");

			WebApp.Start<Api.Startup>(address);
			return address;
		}

		private static RestSharp.RestClient GetClient()
		{
			var client = new RestSharp.RestClient(_hostAddress.Value);
			return client;
		}

		protected RestRequest AdminRequest(string resource, Method method)
		{
			var request = new RestRequest(resource, method) {RequestFormat = DataFormat.Json};
			var loggedInResponse = _adminUser.Value;
			var format = string.Format("{0} {1}", loggedInResponse.TokenType, loggedInResponse.AccessToken);
			request.AddHeader("Authorization", format);
			return request;
		}

		private static TokenResponseModel LoggedInResponse()
		{
		    var oAuthConnection = new OAuthApiClient(_adminRequestFactory.Value);
		    var generateToken = oAuthConnection.GenerateToken(new TokenRequestModel()
		    {
		        UserName = AdminUser,
		        client_id = ClientId,
		        Password = AdminPassword
		    }).Result;
		    var request = new RestRequest("Token", Method.POST);
			request.AddParameter("username", AdminUser);
			request.AddParameter("password", AdminPassword);
			request.AddParameter("grant_type", "password");
			request.AddParameter("client_id", ClientId);
			// action

            return generateToken;
		}

		#endregion

		

	}
}