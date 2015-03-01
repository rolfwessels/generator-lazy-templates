using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Api.SignalR.Connnections;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using log4net;

namespace MainSolutionTemplate.Api.SignalR.Hubs
{


    public class UserHub : BaseHub, IUserControllerActions, ISubscribeUpdateModel<UserModel>, IUserControllerStandardLookups
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string UserUpdateGroupName = "user.update";
        private readonly UserCommonController _userCommonController;

        public UserHub(UserCommonController userCommonController, IConnectionStateMapping connectionStateMapping)
            : base(connectionStateMapping)
        {
            _userCommonController = userCommonController;
        }

        #region IUserControllerActions Members

        [HubAuthorizeActivity(Activity.UserGet)]
        public Task<UserModel> Get(Guid id)
        {
            return _userCommonController.Get(id);
        }

        [HubAuthorizeActivity(Activity.UserPost)]
        public Task<UserModel> Post(UserDetailModel user)
        {
            return _userCommonController.Post(user);
        }

        [HubAuthorizeActivity(Activity.UserUpdate)]
        public Task<UserModel> Put(Guid id, UserDetailModel user)
        {
            return _userCommonController.Put(id, user);
        }

        [HubAuthorizeActivity(Activity.UserDelete)]
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

      

        #region Overrides of BaseHub

        protected override void OnInitializeOnce()
        {
            Messenger.Default.Register<DalUpdateMessage<User>>(this,
                                                               (r) =>
                                                                   {
                                                                       OnUpdate(r.ToValueUpdateModel<User, UserModel>());
                                                                   });
        }

        #endregion

        [HubAuthorizeActivity(Activity.UserGet)]
        public async Task<List<UserReferenceModel>> Get()
        {
            var task = await _userCommonController.Get();
            return task.ToList();
        }

        [HubAuthorizeActivity(Activity.UserGet)]
        public async Task<List<UserModel>> GetDetail()
        {
            var task = await _userCommonController.GetDetail();
            return task.ToList();
        }

        #region Implementation of ISubscribeUpdateModel

        [HubAuthorizeActivity(Activity.UserSubscribe)]
        public void OnUpdate(ValueUpdateModel<UserModel> updatedModel)
        {
            _log.Info("Sending update to " + UserUpdateGroupName);
            Clients.Group(UserUpdateGroupName).OnUpdate(updatedModel);
        }

        [HubAuthorizeActivity(Activity.UserSubscribe)]
        public void SubscribeToUpdate()
        {
            _log.Info("Adding user to " + UserUpdateGroupName);
            Groups.Add(Context.ConnectionId, UserUpdateGroupName);
        }

        [HubAuthorizeActivity(Activity.UserSubscribe)]
        public void UnsubscribeFromUpdate()
        {
            _log.Info("Remove user from " + UserUpdateGroupName);
            Groups.Remove(Context.ConnectionId, UserUpdateGroupName);
        }

        #endregion
    }
}