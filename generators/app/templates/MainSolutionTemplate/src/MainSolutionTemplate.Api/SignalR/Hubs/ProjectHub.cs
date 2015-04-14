using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Api.SignalR.Connection;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
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

        [HubAuthorizeActivity(Activity.ProjectGet)]
        public Task<ProjectModel> Get(Guid id)
        {
            return _projectCommonController.Get(id);
        }

        [HubAuthorizeActivity(Activity.ProjectPost)]
        public Task<ProjectModel> Post(ProjectDetailModel project)
        {
            return _projectCommonController.Post(project);
        }

        [HubAuthorizeActivity(Activity.ProjectUpdate)]
        public Task<ProjectModel> Put(Guid id, ProjectDetailModel project)
        {
            return _projectCommonController.Put(id, project);
        }

        [HubAuthorizeActivity(Activity.ProjectDelete)]
        public Task<bool> Delete(Guid id)
        {
            return _projectCommonController.Delete(id);
        }

        #endregion

        #region IEventUpdateEvent Members

        [HubAuthorizeActivity(Activity.ProjectSubscribe)]
        public async Task SubscribeToUpdates()
        {
            RegisterForDalUpdates<Project, ProjectModel>(OnUpdate);
            await Groups.Add(Context.ConnectionId, UpdateGroupName);
        }


        [HubAuthorizeActivity(Activity.ProjectSubscribe)]
        public async Task OnUpdate(ValueUpdateModel<ProjectModel> project)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
            await context.Clients.Group(UpdateGroupName).OnUpdate(project);
        }

        [HubAuthorizeActivity(Activity.ProjectSubscribe)]
        public async Task UnsubscribeFromUpdates()
        {
            UnregisterFromDalUpdates<Project>();
            await Groups.Remove(Context.ConnectionId, UpdateGroupName);
        }

        #endregion

        #region Overrides of BaseHub

        protected override void OnInitializeOnce()
        {
           
        }

        #endregion

        [HubAuthorizeActivity(Activity.ProjectGet)]
        public async Task<IList<ProjectReferenceModel>> Get()
        {
            var task = await _projectCommonController.Get();
            return task.ToList();
        }

        [HubAuthorizeActivity(Activity.ProjectGet)]
        public async Task<IList<ProjectModel>> GetDetail()
        {
            var task = await _projectCommonController.GetDetail();
            return task.ToList();
        }
    }
}