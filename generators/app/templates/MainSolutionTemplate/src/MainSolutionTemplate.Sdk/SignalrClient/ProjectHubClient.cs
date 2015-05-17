using MainSolutionTemplate.Sdk.SignalrClient.Base;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Microsoft.AspNet.SignalR.Client;

namespace MainSolutionTemplate.Sdk.SignalrClient
{
    public class ProjectHubClient : BaseCrudHubClient<ProjectModel, ProjectReferenceModel, ProjectDetailModel>, IProjectControllerActions, IProjectControllerStandardLookups, IEventUpdateEventSubSubscription<ProjectModel>
	{
        public ProjectHubClient(HubConnection hubConnection)
            : base(hubConnection, "ProjectHub")
        {
        }

	}
}