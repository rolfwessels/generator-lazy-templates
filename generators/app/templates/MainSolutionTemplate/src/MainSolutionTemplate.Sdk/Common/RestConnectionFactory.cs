using System;
using System.Collections.Generic;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;
using RestSharp.Portable;

namespace MainSolutionTemplate.Sdk.Common
{
    public class RestConnectionFactory
    {
        private readonly string _urlString;

        private readonly Dictionary<string, RestClient> _clients =
            new Dictionary<string, RestClient>();

        public RestConnectionFactory(string urlString)
        {
            _urlString = urlString;
        }

        public RestClient GetClient()
        {
            return _clients.GetOrAdd(_urlString, CreateClient);
        }

        #region Private Methods

        private RestClient CreateClient(string uri)
        {
            return new RestClient(new Uri(uri));
        }

        #endregion
    }
}