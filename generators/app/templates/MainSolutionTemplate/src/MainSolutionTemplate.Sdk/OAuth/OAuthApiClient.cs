using System.Net;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.Helpers;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Shared.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public class OAuthApiClient : RestClientBase
    {
        public OAuthApiClient(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory)
        {
        }

        public async Task<TokenResponseModel> GenerateToken(TokenRequestModel tokenRequestModel)
        {
            var request = new RestRequest("Token", Method.POST);
            request.AddParameter("username", tokenRequestModel.UserName);
            request.AddParameter("password", tokenRequestModel.Password);
            request.AddParameter("grant_type", tokenRequestModel.GrantType);
            request.AddParameter("client_id", tokenRequestModel.client_id);

            IRestResponse<TokenResponseModel> result =
                await _restClient.ExecuteAsyncWithLogging<TokenResponseModel>(request);
            ValidateResponse(result);
            var bearerToken = string.Format("{0} {1}", result.Data.TokenType, result.Data.AccessToken);
            _restClient.DefaultParameters.Add(new Parameter() { Type = ParameterType.HttpHeader, Name = "Authorization", Value = bearerToken });
            return result.Data;
        }

        #region Private Methods

        #endregion

        #region Nested type: ErrorResponse

        public class ErrorResponse
        {
            public string Error { get; set; }
            public string Error_description { get; set; }
        }

        #endregion

        #region Overrides of RestClientBase

        protected override void ValidateResponse<T>(IRestResponse<T> result)
        {
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = JsonConvert.DeserializeObject<ErrorResponse>(result.Content);
                throw new RestClientException(errorMessage.Error_description, result);
            }
        }

        #endregion
    }
}