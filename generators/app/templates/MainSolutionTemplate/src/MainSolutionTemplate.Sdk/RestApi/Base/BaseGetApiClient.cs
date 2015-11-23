using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.Models;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Models.Interfaces;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;

namespace MainSolutionTemplate.Sdk.RestApi.Base
{
    public class BaseGetApiClient<TModel, TReferenceModel> : ApiClientBase,
                                                             IBaseStandardLookups<TModel, TReferenceModel>
        where TModel : IBaseModel, new()
    {
        public BaseGetApiClient(RestConnectionFactory restConnectionFactory, string userController)
            : base(restConnectionFactory, userController)
        {
        }

        #region Implementation of IBaseStandardLookups<UserModel,UserReferenceModel>

        public Task<IEnumerable<TReferenceModel>> Get()
        {
            return Get("");
        }

        public Task<IEnumerable<TModel>> GetDetail()
        {
            return GetDetail("");
        }

        #endregion

        public async Task<PagedResult<TReferenceModel>> GetPaged(string oDataQuery)
        {
            if (oDataQuery == null || !oDataQuery.Contains("$inlinecount"))
                oDataQuery = string.Format("{0}&$inlinecount=allpages", oDataQuery);
            RestRequest request = DefaultRequest(_apiPrefix + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<PagedResult<TReferenceModel>>(request);
        }

        public async Task<IEnumerable<TReferenceModel>> Get(string oDataQuery)
        {
            RestRequest request = DefaultRequest(_apiPrefix + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<List<TReferenceModel>>(request);
        }

        public async Task<IEnumerable<TModel>> GetDetail(string oDataQuery)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithDetail) + "?" + oDataQuery,
                                                 Method.GET);
            return await ExecuteAndValidate<List<TModel>>(request);
        }

        public async Task<PagedResult<TModel>> GetDetailPaged(string oDataQuery)
        {
            if (oDataQuery == null || !oDataQuery.Contains("$inlinecount"))
                oDataQuery = string.Format("{0}&$inlinecount=allpages", oDataQuery);
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithDetail) + "?" + oDataQuery,
                                                 Method.GET);
            return await ExecuteAndValidate<PagedResult<TModel>>(request);
        }
    }
}