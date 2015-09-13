using MainSolutionTemplate.Sdk.RestApi.Base;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Sdk.RestApi
{
    public class ProjectApiClient : BaseCrudApiClient<ProjectModel,ProjectDetailModel,ProjectReferenceModel>, IProjectControllerActions
    {
        public ProjectApiClient(RestConnectionFactory restConnectionFactory)
            : base(restConnectionFactory, RouteHelper.ProjectController)
        {
        }

    }
}
