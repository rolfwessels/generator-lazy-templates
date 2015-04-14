using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Microsoft.AspNet.SignalR.Client;

namespace MainSolutionTemplate.Sdk.SignalrClient
{
    public class ProjectHubClient : IProjectControllerActions, IProjectControllerStandardLookups, IEventUpdateEventSubSubscription<ProjectModel>
	{
		private readonly IHubProxy _projectHub;

		public ProjectHubClient(HubConnection hubConnection)
		{
			_projectHub = hubConnection.CreateHubProxy("ProjectHub");
		}

		#region Implementation of IProjectHub
		
		public Task<ProjectModel> Get(Guid id)
		{
			return _projectHub.Invoke<ProjectModel>("Get", id);
		}

		public Task<ProjectModel> Post(ProjectDetailModel project)
		{
			return _projectHub.Invoke<ProjectModel>("Post", project);
		}

		public Task<ProjectModel> Put(Guid id, ProjectDetailModel project)
		{
			return _projectHub.Invoke<ProjectModel>("Put", id, project);
		}

		public Task<bool> Delete(Guid id)
		{
			return _projectHub.Invoke<bool>("Delete", id);
		}
        
	    #endregion

	    #region Implementation of IProjectControllerStandardLookups

	    public Task<IList<ProjectReferenceModel>> Get()
	    {
            return _projectHub.Invoke<IList<ProjectReferenceModel>>("Get");
	    }

	    public Task<IList<ProjectModel>> GetDetail()
	    {
            return _projectHub.Invoke<IList<ProjectModel>>("GetDetail");
	    }

	    #endregion

        #region Implementation of IEventUpdateEvent

        public Task OnUpdate(Action<ValueUpdateModel<ProjectModel>> callBack)
        {
           return  Task.Run(() => _projectHub.On("OnUpdate", callBack));
        }

        public Task SubscribeToUpdates()
        {
            return  Task.Run(() => _projectHub.Invoke("SubscribeToUpdates"));
        }

        public Task UnsubscribeFromUpdates()
        {
            return  Task.Run(() => _projectHub.Invoke("UnsubscribeFromUpdates"));
        }

        #endregion
	}
}