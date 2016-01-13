using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class OAuthDataManager : IOAuthDataManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IApplicationManager _applicationManager;
        private readonly IUserManager _userManager;

        public OAuthDataManager(IUserManager userManager, IApplicationManager applicationManager)
        {
            _userManager = userManager;
            _applicationManager = applicationManager;
        }

        #region Implementation of IOAuthDataManager

        public Task<IOAuthClient> GetApplication(string clientId)
        {
            return Task.Run(() => _applicationManager.GetApplicationById(clientId).MapToIOAuthClient());
        }

        public async Task<IAuthorizedUser> GetUserByUserIdAndPassword(string userName, string password)
        {
            _log.Info(string.Format("Login user '{0}'", userName));
            User user = await _userManager.GetUserByEmailAndPassword(userName, password);
            return user.MapToIAuthorizedUser();
        }

        public async Task<string[]> GetRolesForUser(IAuthorizedUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            _log.Info(string.Format("Roles user '{0}'", user.UserId));
            User foundUser = await _userManager.GetUserByEmail(user.UserId);
            return foundUser != null ? foundUser.Roles.ToArray() : new string[0];
        }

        public Task UpdateUserLastActivityDate(IAuthorizedUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.Run(() =>
                {
                    _log.Info(string.Format("Roles user '{0}'", user.UserId));
                    _userManager.UpdateLastLoginDate(user.UserId);
                });
        }

        #endregion
    }
}