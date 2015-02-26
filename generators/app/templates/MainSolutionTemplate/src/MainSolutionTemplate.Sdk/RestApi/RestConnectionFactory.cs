using System;
using System.Collections.Concurrent;
using RestSharp;

namespace MainSolutionTemplate.Sdk.RestApi
{
    public class RestConnectionFactory
    {
        private readonly string _urlString;

        private readonly ConcurrentDictionary<string, RestClient> _clients =
            new ConcurrentDictionary<string, RestClient>();

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