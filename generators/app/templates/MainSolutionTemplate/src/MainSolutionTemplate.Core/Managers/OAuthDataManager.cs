using System;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.Core.Vendor.PasswordHash;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using System.Linq;
using log4net;

namespace MainSolutionTemplate.Core.Managers
{
	public class OAuthDataManager : IOAuthDataManager
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IGeneralUnitOfWork _generalUnitOfWork;

		public OAuthDataManager(IGeneralUnitOfWork generalUnitOfWork )
		{
			_generalUnitOfWork = generalUnitOfWork;
		}

		#region Implementation of IRefreshTokenManager

		public IRefreshToken CreateRefresherToken()
		{
			throw new System.NotImplementedException();
		}

		public Task SaveRefreshToken(IRefreshToken token)
		{
			throw new System.NotImplementedException();
		}

		public Task<IRefreshToken> GetRefreshToken(string hashedTokenId)
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteRefreshToken(string hashedTokenId)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region Implementation of IOAuthDataManager

		public Task<IOAuthClient> GetClient(string clientId)
		{
			return Task.Run(() => _generalUnitOfWork.Applications.FirstOrDefault(x => x.ClientId == clientId).MapToIOAuthClient());
		}

		public Task<IAuthorizedUser> GetUserByUserNameAndPassword(string userName, string password)
		{
			if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");
			if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("password");
			return Task.Run(() =>
				{
					_log.Info(string.Format("Loggin user '{0}'", userName));
					User user = _generalUnitOfWork.Users.FirstOrDefault(x => x.Email.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
					if (user != null)
					{
						if (!PasswordHash.ValidatePassword(password, user.HashedPassword))
						{
							user = null;
							_log.Info(string.Format("Invalid password for user '{0}'", userName));
						}
					}
					else
					{
						_log.Info(string.Format("Invalid user '{0}'", userName));
					}
					return user.MapToIAuthorizedUser();
				});
		}

		public Task<string[]> GetRolesForUser(IAuthorizedUser authorizedUser)
		{
			return Task.Run(() =>
			{
				_log.Info(string.Format("Roles user '{0}'", authorizedUser.UserId));
				var user = _generalUnitOfWork.Users.FirstOrDefault(x => x.Email.Equals(authorizedUser.UserId, StringComparison.InvariantCultureIgnoreCase));
				return user != null ? user.Roles.Select(x => x.Name).ToArray() : new string[0];
			});
		}

		public Task UpdateUserLastActivityDate(IAuthorizedUser users)
		{
			return Task.Run(() =>
			{
				_log.Info(string.Format("Roles user '{0}'", users.UserId));
				var user = _generalUnitOfWork.Users.FirstOrDefault(x => x.Email.Equals(users.UserId, StringComparison.InvariantCultureIgnoreCase));
				user.LastLoginDate = DateTime.Now;
				_generalUnitOfWork.Users.Update(user);
			});
		}

		#endregion
	}
}