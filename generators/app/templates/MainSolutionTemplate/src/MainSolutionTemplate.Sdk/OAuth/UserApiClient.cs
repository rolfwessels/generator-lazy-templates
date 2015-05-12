using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.Models;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;

namespace MainSolutionTemplate.Sdk.OAuth
{
    public class UserApiClient : ApiClientBase, IUserControllerActions, IUserControllerStandardLookups
    {
        private readonly string _apiPrefix;

        public UserApiClient(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory)
        {
            _apiPrefix = RouteHelper.UserController;
        }

        #region Implementation of IUserControllerActions

        public async Task<UserModel> Get(Guid id)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<UserModel> Post(UserDetailModel model)
        {
            var request = DefaultRequest(_apiPrefix, Method.POST); 
            request.AddBody(model);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<UserModel> Put(Guid id, UserDetailModel model)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.PUT);
            request.AddUrlSegment("id", id.ToString());
            request.AddBody(model);
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

        public  Task<IList<UserReferenceModel>> Get()
        {
            return Get("");
        }

        public Task<IList<UserModel>> GetDetail()
        {
            return GetDetail("");
        }

        #endregion

        public async Task<PagedResult<UserReferenceModel>> GetPaged(string oDataQuery)
        {
            if (oDataQuery == null || !oDataQuery.Contains("$inlinecount"))
                oDataQuery = string.Format("{0}&$inlinecount=allpages", oDataQuery);
            var request = DefaultRequest(_apiPrefix + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<PagedResult<UserReferenceModel>>(request);
        }

        public async Task<IList<UserReferenceModel>> Get(string oDataQuery)
        {
            var request = DefaultRequest(_apiPrefix + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<List<UserReferenceModel>>(request);
        }

        public async Task<IList<UserModel>> GetDetail(string oDataQuery)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithDetail) + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<List<UserModel>>(request);
        }

        public async Task<PagedResult<UserModel>> GetDetailPaged(string oDataQuery)
        {
            if (oDataQuery == null || !oDataQuery.Contains("$inlinecount"))
                oDataQuery = string.Format("{0}&$inlinecount=allpages", oDataQuery);
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithDetail) + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<PagedResult<UserModel>>(request);
        }

        #region Private Methods
        
        private RestRequest DefaultRequest(string userController, Method get)
        {
            return new RestRequest(userController, get) { RequestFormat = DataFormat.Json };
        }

        #endregion
    }
}
