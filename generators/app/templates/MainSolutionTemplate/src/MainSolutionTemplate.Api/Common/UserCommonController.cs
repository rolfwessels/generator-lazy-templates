using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models;
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

        
        public Task<IQueryable<UserReferenceModel>> Get()
        {
            return Task.Run(() => _systemManager.GetUsersAsReference().ToUserModel());
        }

        public Task<IQueryable<UserModel>> GetDetail()
        {
            return Task.Run(() => _systemManager.GetUsers().ToUserModel());
        }

        public Task<UserModel> Get(Guid id)
        {
            return Task.Run(() => _systemManager.GetUser(id).ToUserModel());
        }

        public Task<UserModel> Put(Guid id, UserDetailModel user)
        {
            return Task.Run(() =>
            {
                var userFound = _systemManager.GetUser(id);
                if (userFound == null) throw new Exception(string.Format("Could not find model by id '{0}'", id));
                var saveUser = _systemManager.SaveUser(user.ToUser(userFound));
                return saveUser.ToUserModel();
            });
        }

       
        public Task<UserModel> Post(UserDetailModel user)
        {
            return Task.Run(() =>
            {
                var savedUser = _systemManager.SaveUser(user.ToUser());
                return savedUser.ToUserModel();
            });
        }

        
        public Task<bool> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                var deleteUser = _systemManager.DeleteUser(id);
                return deleteUser != null;
            });
        }

        #region Other actions

        public Task<UserModel> Register(RegisterModel model)
        {
            return Task.Run(() =>
            {
                var user = model.ToUser();
                user.Roles.Add(Roles.Guest);
                var savedUser = _systemManager.SaveUser(user);
                return savedUser.ToUserModel();
            });
        }

        public Task<bool> ForgotPassword(string email)
        {
            return Task.Run(() =>
            {
                _log.Warn(string.Format("User has called forgot password. We should send him and email to [{0}].", email));
                // todo: Forgot password
                return true;
            });
        }

        #endregion
    }

   
}