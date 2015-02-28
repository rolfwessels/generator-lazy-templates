using System;
using RestSharp;
using RestSharp.Portable;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public class RestClientException : Exception
    {
        private readonly IRestResponse _response;

        public RestClientException(string message, IRestResponse response) : base(message)
        {
            _response = response;
        }

        public IRestResponse Response
        {
            get { return _response; }
        }
    }
}