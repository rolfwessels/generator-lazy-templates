using System;
using System.Net;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.Helpers;
using MainSolutionTemplate.Sdk.RestApi;
using Newtonsoft.Json;
using RestSharp;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public class RestClientBase
    {
        protected RestSharp.RestClient _restClient;

        public RestClientBase(RestConnectionFactory restConnectionFactory)
        {
            _restClient = restConnectionFactory.GetClient();
        }

        protected virtual void ValidateResponse<T>(IRestResponse<T> result)
        {
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = JsonConvert.DeserializeObject<OAuthApiClient.ErrorResponse>(result.Content);
                throw new RestClientException(errorMessage.Error_description, result);
            }
        }

        protected async Task<T> ExecuteAndValidate<T>(RestRequest request) where T : new()
        {
            var response = await _restClient.ExecuteAsyncWithLogging<T>(request);
            ValidateResponse(response);
            return response.Data;
        } 
        
        protected async Task<bool> ExecuteAndValidateBool(RestRequest request) 
        {
            var response = await _restClient.ExecuteAsyncWithLogging<bool>(request);
            ValidateResponse(response);
            return Convert.ToBoolean(response.Content);
        }
    }
}