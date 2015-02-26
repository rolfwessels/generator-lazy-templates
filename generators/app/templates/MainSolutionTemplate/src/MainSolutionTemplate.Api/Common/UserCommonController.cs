using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.WebApi.Attributes;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using log4net;

namespace MainSolutionTemplate.Api.Common
{
    
    public class UserCommonController : IUserControllerActions
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISystemManager _systemManager;

        public UserCommonController(ISystemManager systemManager)
        {
            _systemManager = systemManager;

        }

        [AuthorizeActivity(Activity.UserGet)]
        public IQueryable<UserReferenceModel> Get()
        {
            return _systemManager.GetUsersAsReference().ToUserModel();
        }

        [AuthorizeActivity(Activity.UserGet)]
        public IQueryable<UserModel> GetDetail()
        {
            return _systemManager.GetUsers().ToUserModel();
        }

        [AuthorizeActivity(Activity.UserGet)]
        public UserModel Get(Guid id)
        {
            return _systemManager.GetUser(id).ToUserModel();
        }

        [AuthorizeActivity(Activity.UserUpdate)]
        public UserModel Put(Guid id, UserDetailModel user)
        {
            var userFound = _systemManager.GetUser(id);
            if (userFound == null) throw new Exception(string.Format("Could not find model by id '{0}'", id));
            var saveUser = _systemManager.SaveUser(user.ToUser(userFound));
            return saveUser.ToUserModel();
        }

       
        [AuthorizeActivity(Activity.UserPost)]
        public UserModel Post(UserDetailModel user)
        {
            var savedUser = _systemManager.SaveUser(user.ToUser());
            return savedUser.ToUserModel();
        }

        
        [AuthorizeActivity(Activity.UserDelete)]
        public bool Delete(Guid id)
        {
            var deleteUser = _systemManager.DeleteUser(id);
            return deleteUser != null;
        }

        #region Other actions

        [AllowAnonymous]
        [Route(RouteHelper.UserControllerRegister)]
        [HttpPost]
        public UserModel Register(RegisterModel model)
        {
            var user = model.ToUser();
            user.Roles.Add(Roles.Guest);
            var savedUser = _systemManager.SaveUser(user);
            return savedUser.ToUserModel();
        }

       
        [AllowAnonymous]
        [Route(RouteHelper.UserControllerForgotPassword)]
        [HttpGet]
        public bool ForgotPassword(string email)
        {
            _log.Warn(string.Format("User has called forgot password. We should send him and email to [{0}].", email));
            // todo: Rolf Forgot password
            return true;
        }

        #endregion
    }

   
}