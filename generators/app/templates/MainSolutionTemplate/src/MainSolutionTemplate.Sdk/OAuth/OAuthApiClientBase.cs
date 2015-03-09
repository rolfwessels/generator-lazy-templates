using System.Net;
using MainSolutionTemplate.Sdk.RestApi;
using Newtonsoft.Json;
using RestSharp;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public  abstract class OAuthApiClientBase : ApiClientBase
    {
        protected OAuthApiClientBase(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory)
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