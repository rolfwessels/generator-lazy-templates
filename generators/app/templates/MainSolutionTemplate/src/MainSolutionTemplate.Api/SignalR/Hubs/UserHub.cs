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
    

    public class UserHub : BaseHub, IUserControllerActions, IEventUpdateEvent<UserModel>, IUserControllerStandardLookups
    {
        private const string UpdateGroupName = "user.update.group";
        private readonly UserCommonController _userCommonController;

        public UserHub(UserCommonController userCommonController, IConnectionStateMapping connectionStateMapping)
            : base(connectionStateMapping)
        {
            _userCommonController = userCommonController;
        }

        #region IUserControllerActions Members

        [HubAuthorizeActivity(Activity.ReadUsers)]
        public Task<UserModel> Get(Guid id)
        {
            return _userCommonController.Get(id);
        }

        [HubAuthorizeActivity(Activity.InsertUsers)]
        public Task<UserModel> Insert(UserDetailModel model)
        {
            return _userCommonController.Insert(model);
        }

        [HubAuthorizeActivity(Activity.UpdateUsers)]
        public Task<UserModel> Update(Guid id, UserDetailModel model)
        {
            return _userCommonController.Update(id, model);
        }

        [HubAuthorizeActivity(Activity.DeleteUser)]
        public Task<bool> Delete(Guid id)
        {
            return _userCommonController.Delete(id);
        }


        public Task<UserModel> Register(RegisterModel user)
        {
            return _userCommonController.Register(user);
        }


        public Task<bool> ForgotPassword(string email)
        {
            return _userCommonController.ForgotPassword(email);
        }

        #endregion

        #region IEventUpdateEvent Members

        [HubAuthorizeActivity(Activity.SubscribeUser)]
        public async Task SubscribeToUpdates()
        {
            RegisterForDalUpdates<User, UserModel>(OnUpdate);
            await Groups.Add(Context.ConnectionId, UpdateGroupName);
        }

        [HubAuthorizeActivity(Activity.SubscribeUser)]
        public async Task OnUpdate(ValueUpdateModel<UserModel> user)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
            await context.Clients.Group(UpdateGroupName).OnUpdate(user);
        }

        [HubAuthorizeActivity(Activity.SubscribeUser)]
        public async Task UnsubscribeFromUpdates()
        {
            UnregisterFromDalUpdates<User>();
            await Groups.Remove(Context.ConnectionId, UpdateGroupName);
        }

        #endregion
        
        [HubAuthorizeActivity(Activity.ReadUsers)]
        public async Task<IList<UserReferenceModel>> Get()
        {
            var task = await _userCommonController.Get();
            return task.ToList();
        }

        [HubAuthorizeActivity(Activity.ReadUsers)]
        public async Task<IList<UserModel>> GetDetail()
        {
            var task = await _userCommonController.GetDetail();
            return task.ToList();
        }
    }
}