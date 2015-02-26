using System;
using System.Collections.Generic;
using System.Linq;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.SignalR
{
    
    public class UserHub : BaseHub, IUserControllerActions, IUserHubEvents
    {
        private readonly UserCommonController _userCommonController;

        public UserHub(UserCommonController userCommonController, IConnectionStateMapping connectionStateMapping)
            : base(connectionStateMapping)
        {
            _userCommonController = userCommonController;
        }

        #region IUserControllerActions Members

        [HubAuthorizeActivity(Activity.UserGet)]
        public UserModel Get(Guid id)
        {
            return _userCommonController.Get(id);
        }

        [HubAuthorizeActivity(Activity.UserPost)]
        public UserModel Post(UserDetailModel user)
        {
            UserModel userModel = _userCommonController.Post(user);
            return userModel;
        }

        [HubAuthorizeActivity(Activity.UserUpdate)]
        public UserModel Put(Guid id, UserDetailModel user)
        {
            return _userCommonController.Put(id, user);
        }

        [HubAuthorizeActivity(Activity.UserDelete)]
        public bool Delete(Guid id)
        {
            return _userCommonController.Delete(id);
        }


        public UserModel Register(RegisterModel user)
        {
            return _userCommonController.Register(user);
        }


        public bool ForgotPassword(string email)
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
        public List<UserReferenceModel> Get()
        {
            return _userCommonController.Get().ToList();
        }

        [HubAuthorizeActivity(Activity.UserGet)]
        public List<UserModel> GetDetail()
        {
            return _userCommonController.GetDetail().ToList();
        }
    }
}