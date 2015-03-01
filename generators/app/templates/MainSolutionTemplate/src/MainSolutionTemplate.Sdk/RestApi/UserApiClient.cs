using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MainSolutionTemplate.Sdk.Common;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;
using RestSharp.Portable;
using RestSharp.Portable.Serializers;
using System.Linq;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public class UserApiClient : RestClientBase, IUserControllerActions, IUserControllerStandardLookups
    {
        private readonly string _apiPrefix;

        public UserApiClient(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory)
        {
            _apiPrefix = RouteHelper.UserController;
        }

        private RestRequest DefaultRequest(string userController, HttpMethod get)
        {
            return new RestRequest(userController, get) { };
        }

        #region Implementation of IUserControllerActions

        public async Task<UserModel> Get(Guid id)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), HttpMethod.Get);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<UserModel> Post(UserDetailModel user)
        {
            var request = DefaultRequest(_apiPrefix, HttpMethod.Post); 
            request.AddBody(user);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<UserModel> Put(Guid id, UserDetailModel user)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), HttpMethod.Put);
            request.AddUrlSegment("id", id.ToString());
            request.AddBody(user);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<bool> Delete(Guid id)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), HttpMethod.Delete);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidateBool(request);
        }

        public async Task<UserModel> Register(RegisterModel user)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.UserControllerRegister), HttpMethod.Post);
            request.AddBody(user);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.UserControllerForgotPassword), HttpMethod.Get);
            request.AddUrlSegment("email", email);
            return await ExecuteAndValidateBool(request);
        }

        #endregion


        public async Task<List<UserReferenceModel>> Get(string oDataQuery)
        {
            var request = DefaultRequest(_apiPrefix + "?" + oDataQuery, HttpMethod.Get);
            return await ExecuteAndValidate<List<UserReferenceModel>>(request);
        }


        #region Implementation of IUserControllerStandardLookups

        public async Task<List<UserReferenceModel>> Get()
        {
//            var token = _restClient.DefaultParameters.Where(x => x.Name == "Authorization").Select(x => x.Value as string).First().Split(' ').Last();
//            var withOAuthBearerToken = _restClient.BaseUrl.ToString().AppendPathSegment(_apiPrefix).WithOAuthBearerToken(token);
//            return await withOAuthBearerToken.GetJsonAsync<IList<UserReferenceModel>>();

            var request = DefaultRequest(_apiPrefix, HttpMethod.Get);
            var userReferenceModels = await ExecuteAndValidate<UserReferenceModel[]>(request);
            return userReferenceModels.ToList();
        }

        public async Task<List<UserModel>> GetDetail()
        {
            var request = DefaultRequest(_apiPrefix, HttpMethod.Get);
            return await ExecuteAndValidate<List<UserModel>>(request);
        }

        #endregion

    }
}
