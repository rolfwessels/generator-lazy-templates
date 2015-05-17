using System;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Models.Interfaces;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;

namespace MainSolutionTemplate.Sdk.RestApi.Base
{
    public class BaseCrudApiClient<TModel, TDetailModel, TReferenceModel> : BaseGetApiClient<TModel, TReferenceModel>,
                                                                            ICrudController<TModel, TDetailModel>
        where TModel : IBaseModel, new()
    {
        protected BaseCrudApiClient(RestConnectionFactory restConnectionFactory, string userController)
            : base(restConnectionFactory, userController)
        {
        }

        #region ICrudController<TModel,TDetailModel> Members

        public async Task<TModel> Get(Guid id)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidate<TModel>(request);
        }

        public async Task<TModel> Insert(TDetailModel model)
        {
            RestRequest request = DefaultRequest(_apiPrefix, Method.POST);
            request.AddBody(model);
            return await ExecuteAndValidate<TModel>(request);
        }

        public async Task<TModel> Update(Guid id, TDetailModel model)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.PUT);
            request.AddUrlSegment("id", id.ToString());
            request.AddBody(model);
            return await ExecuteAndValidate<TModel>(request);
        }

        public async Task<bool> Delete(Guid id)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidateBool(request);
        }

        #endregion
    }
}