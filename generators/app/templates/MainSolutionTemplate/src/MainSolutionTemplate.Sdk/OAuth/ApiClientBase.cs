using System;
using System.Net;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.Helpers;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public  abstract class ApiClientBase
    {
        protected RestClient _restClient;

        protected ApiClientBase(RestConnectionFactory restConnectionFactory)
        {
            _restClient = restConnectionFactory.GetClient();
        }

        protected virtual void ValidateResponse<T>(IRestResponse<T> result)
        {
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = SimpleJson.DeserializeObject<ErrorMessage>(result.Content);
                Console.Out.WriteLine("errorMessage: " + errorMessage.Dump());
                
                throw new Exception(errorMessage.Message);
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