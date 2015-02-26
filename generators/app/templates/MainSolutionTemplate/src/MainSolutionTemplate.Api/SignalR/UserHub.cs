using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.SignalR
{
    

    public class UserHub : BaseHub, IUserControllerActions, IUserHubEvents, IUserControllerStandardLookups
    {
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

        #region IUserHubEvents Members

        [HubAuthorizeActivity(Activity.UserSubscribe)]
        public void OnUpdate(ValueUpdateModel<UserModel> user)
        {
            Clients.All.OnUpdate(user);
        }

        #endregion

        #region Overrides of BaseHub

        protected override void OnInitializeOnce()
        {
            Messenger.Default.Register<DalUpdateMessage<User>>(this,
                                                               (r) =>
                                                                   { OnUpdate(r.ToValueUpdateModel<User, UserModel>());
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
    }
}