using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class OAuthDataManager : IOAuthDataManager
	{
         private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IUserManager _userManager;
        private readonly IApplicationManager _applicationManager;

        public OAuthDataManager(IUserManager userManager , IApplicationManager applicationManager)
        {
            _userManager = userManager;
            _applicationManager = applicationManager;
        }

        #region Implementation of IOAuthDataManager

		public Task<IOAuthClient> GetApplication(string clientId)
		{
            return Task.Run(() => _applicationManager.GetApplicationById(clientId).MapToIOAuthClient());
		}

		public Task<IAuthorizedUser> GetUserByUserIdAndPassword(string userName, string password)
		{
			return Task.Run(() =>
				{
					_log.Info(string.Format("Login user '{0}'", userName));
					var user = _userManager.GetUserByEmailAndPassword(userName,password);
					return user.MapToIAuthorizedUser();
				});
		}

		public Task<string[]> GetRolesForUser(IAuthorizedUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.Run(() =>
			{
				_log.Info(string.Format("Roles user '{0}'", user.UserId));
			    var foundUser = _userManager.GetUserByEmail(user.UserId);
				return foundUser != null ? foundUser.Roles.Select(x => x.Name).ToArray() : new string[0];
			});
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