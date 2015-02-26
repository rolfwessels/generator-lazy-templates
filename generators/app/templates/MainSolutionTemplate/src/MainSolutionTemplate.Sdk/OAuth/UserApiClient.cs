using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.Helpers;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public class UserApiClient : OAuthApiClientBase, IUserControllerActions, IUserControllerStandardLookups
    {
        private string _apiPrefix;

        public UserApiClient(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory)
        {
            _apiPrefix = RouteHelper.UserController;
        }


        private RestRequest DefaultRequest(string userController, Method get)
        {
            return new RestRequest(userController, get) { RequestFormat = DataFormat.Json };
        }

        #region Implementation of IUserControllerActions

        public async Task<UserModel> Get(Guid id)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<UserModel> Post(UserDetailModel user)
        {
            var request = DefaultRequest(_apiPrefix, Method.POST); 
            request.AddBody(user);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<UserModel> Put(Guid id, UserDetailModel user)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.PUT);
            request.AddUrlSegment("id", id.ToString());
            request.AddBody(user);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<bool> Delete(Guid id)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidateBool(request);
        }

        public async Task<UserModel> Register(RegisterModel user)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.UserControllerRegister), Method.POST);
            request.AddBody(user);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.UserControllerForgotPassword), Method.GET);
            request.AddUrlSegment("email", email);
            return await ExecuteAndValidateBool(request);
        }

        #endregion

        #region Implementation of IUserControllerStandardLookups

        public async Task<List<UserReferenceModel>> Get()
        {
            var request = DefaultRequest(_apiPrefix, Method.GET);
            return await ExecuteAndValidate<List<UserReferenceModel>>(request);
        }

        public async Task<List<UserModel>> GetDetail()
        {
            var request = DefaultRequest(_apiPrefix, Method.GET);
            return await ExecuteAndValidate<List<UserModel>>(request);
        }

        #endregion
    }
}
