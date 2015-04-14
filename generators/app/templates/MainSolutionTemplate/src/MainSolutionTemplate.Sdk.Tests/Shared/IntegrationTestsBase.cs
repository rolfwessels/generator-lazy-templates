using System;
using System.IO;
using System.Reflection;
using MainSolutionTemplate.Api;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Shared.Models;
using Microsoft.Owin.Hosting;
using NCrunch.Framework;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public class IntegrationTestsBase
    {
        public const string ClientId = "MainSolutionTemplate.Api";
        public const string AdminPassword = "admin!";
        public const string AdminUser = "admin";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Lazy<string> _hostAddress;

        
        protected static Lazy<RestConnectionFactory> _defaultRequestFactory;
        protected static Lazy<RestConnectionFactory> _adminRequestFactory;
        private static TokenResponseModel _adminToken;

        static IntegrationTestsBase()
        {
            _hostAddress = new Lazy<string>(StartHosting);
            _defaultRequestFactory = new Lazy<RestConnectionFactory>(() => new RestConnectionFactory(_hostAddress.Value));
            _adminRequestFactory = new Lazy<RestConnectionFactory>(CreateAdminRequest);
        }

        

        public string SignalRUri
        {
            get { return _defaultRequestFactory.Value.GetClient().BuildUri(new RestRequest("signalr")).ToString(); }
        }

        public static TokenResponseModel AdminToken
        {
            get
            {
                if (_adminToken == null)
                {
                    _log.Info("Create admin request: " + _adminRequestFactory.Value);
                }

                return _adminToken;
            }

        }

        #region Private Methods

        private static string StartHosting()
        {
            int port = new Random().Next(9000, 9999);
            string address = string.Format("http://localhost:{0}/", port);
            _log.Info(string.Format("Starting api on [{0}]", address));

            var combine = Path.GetFullPath(Path.Combine(new Uri(Assembly.GetAssembly(typeof(Startup)).CodeBase).LocalPath, @"..\..\..\"));
            _log.Info("lookfor:" + combine);
            SimpleFileServer.PossibleWebBasePath = Path.Combine(NCrunchEnvironment.GetOriginalSolutionPath()
                ?? combine,
                                                                @"..\MainSolutionTemplate.Website");
            WebApp.Start<Startup>(address);
            return address;
        }


        private static RestConnectionFactory CreateAdminRequest()
        {
            var restConnectionFactory = new RestConnectionFactory(_hostAddress.Value);
            var oAuthConnection = new OAuthApiClient(restConnectionFactory);
            _adminToken = oAuthConnection.GenerateToken(new TokenRequestModel
                {
                    UserName = AdminUser, ClientId = ClientId, Password = AdminPassword
                }).Result;
            
            return restConnectionFactory;
        }

        #endregion
    }
}