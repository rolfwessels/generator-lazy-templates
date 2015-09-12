using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Api.SignalR.Connection;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Api.SignalR.Hubs
{
    

    public class ProjectHub : BaseHub, IProjectControllerActions, IEventUpdateEvent<ProjectModel>, IProjectControllerStandardLookups
    {
        private const string UpdateGroupName = "project.update.group";
        private readonly ProjectCommonController _projectCommonController;

        public ProjectHub(ProjectCommonController projectCommonController, IConnectionStateMapping connectionStateMapping)
            : base(connectionStateMapping)
        {
            _projectCommonController = projectCommonController;
        }

        #region IProjectControllerActions Members

        [HubAuthorizeActivity(Activity.ReadProject)]
        public Task<ProjectModel> Get(Guid id)
        {
            return _projectCommonController.Get(id);
        }

        [HubAuthorizeActivity(Activity.InsertProject)]
        public Task<ProjectModel> Insert(ProjectDetailModel model)
        {
            return _projectCommonController.Insert(model);
        }

        [HubAuthorizeActivity(Activity.UpdateProject)]
        public Task<ProjectModel> Update(Guid id, ProjectDetailModel model)
        {
            return _projectCommonController.Update(id, model);
        }

        [HubAuthorizeActivity(Activity.DeleteProject)]
        public Task<bool> Delete(Guid id)
        {
            return _projectCommonController.Delete(id);
        }

        #endregion

        #region IEventUpdateEvent Members

        [HubAuthorizeActivity(Activity.SubscribeProject)]
        public async Task SubscribeToUpdates()
        {
            RegisterForDalUpdates<Project, ProjectModel>(OnUpdate);
            await Groups.Add(Context.ConnectionId, UpdateGroupName);
        }


        [HubAuthorizeActivity(Activity.SubscribeProject)]
        public async Task OnUpdate(ValueUpdateModel<ProjectModel> project)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
            await context.Clients.Group(UpdateGroupName).OnUpdate(project);
        }

        [HubAuthorizeActivity(Activity.SubscribeProject)]
        public async Task UnsubscribeFromUpdates()
        {
            UnregisterFromDalUpdates<Project>();
            await Groups.Remove(Context.ConnectionId, UpdateGroupName);
        }

        #endregion
        
        [HubAuthorizeActivity(Activity.ReadProject)]
        public async Task<IList<ProjectReferenceModel>> Get()
        {
            var task = await _projectCommonController.Get();
            return task.ToList();
        }

        [HubAuthorizeActivity(Activity.ReadProject)]
        public async Task<IList<ProjectModel>> GetDetail()
        {
            var task = await _projectCommonController.GetDetail();
            return task.ToList();
        }
    }
}