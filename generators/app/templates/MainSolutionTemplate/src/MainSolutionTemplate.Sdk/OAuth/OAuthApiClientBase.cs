using System.Net;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Shared;
using Newtonsoft.Json;
using RestSharp;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public  abstract class OAuthApiClientBase : ApiClientBase
    {
        protected OAuthApiClientBase(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory, RouteHelper.UserController)
        {
        }

        protected override void ValidateResponse<T>(IRestResponse<T> result)
        {
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = JsonConvert.DeserializeObject<OAuthApiClient.ErrorResponse>(result.Content);
                throw new RestClientException(errorMessage.Error_description, result);
            }
        }

        
    }
}