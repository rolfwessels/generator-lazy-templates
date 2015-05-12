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
    public class ProjectApiClient : ApiClientBase, IProjectControllerActions, IProjectControllerStandardLookups
    {
        private readonly string _apiPrefix;

        public ProjectApiClient(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory)
        {
            _apiPrefix = RouteHelper.ProjectController;
        }


        private RestRequest DefaultRequest(string projectController, Method get)
        {
            return new RestRequest(projectController, get) { RequestFormat = DataFormat.Json };
        }

        #region Implementation of IProjectControllerActions

        public async Task<ProjectModel> Get(Guid id)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidate<ProjectModel>(request);
        }

        public async Task<ProjectModel> Post(ProjectDetailModel model)
        {
            var request = DefaultRequest(_apiPrefix, Method.POST); 
            request.AddBody(model);
            return await ExecuteAndValidate<ProjectModel>(request);
        }

        public async Task<ProjectModel> Put(Guid id, ProjectDetailModel model)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.PUT);
            request.AddUrlSegment("id", id.ToString());
            request.AddBody(model);
            return await ExecuteAndValidate<ProjectModel>(request);
        }

        public async Task<bool> Delete(Guid id)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidateBool(request);
        }
        
        #endregion



        #region Implementation of IProjectControllerStandardLookups

        public  Task<IList<ProjectReferenceModel>> Get()
        {
            return Get("");
        }

        public Task<IList<ProjectModel>> GetDetail()
        {
            return GetDetail("");
        }

        #endregion

        public async Task<PagedResult<ProjectReferenceModel>> GetPaged(string oDataQuery)
        {
            if (oDataQuery == null || !oDataQuery.Contains("$inlinecount"))
                oDataQuery = string.Format("{0}&$inlinecount=allpages", oDataQuery);
            var request = DefaultRequest(_apiPrefix + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<PagedResult<ProjectReferenceModel>>(request);
        }

        public async Task<IList<ProjectReferenceModel>> Get(string oDataQuery)
        {
            var request = DefaultRequest(_apiPrefix + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<List<ProjectReferenceModel>>(request);
        }

        public async Task<IList<ProjectModel>> GetDetail(string oDataQuery)
        {
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithDetail) + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<List<ProjectModel>>(request);
        }

        public async Task<PagedResult<ProjectModel>> GetDetailPaged(string oDataQuery)
        {
            if (oDataQuery == null || !oDataQuery.Contains("$inlinecount"))
                oDataQuery = string.Format("{0}&$inlinecount=allpages", oDataQuery);
            var request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithDetail) + "?" + oDataQuery, Method.GET);
            return await ExecuteAndValidate<PagedResult<ProjectModel>>(request);
        }
    }
}
