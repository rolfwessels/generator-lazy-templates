using System;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using System.Linq;
using log4net;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManagerFacade : IOAuthDataManager
	{
		

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

		public Task<IOAuthClient> GetApplication(string clientId)
		{
			if (clientId == null) throw new ArgumentNullException("clientId");
			return Task.Run(() => _generalUnitOfWork.Applications.FirstOrDefault(x => x.ClientId == clientId).MapToIOAuthClient());
		}

		public Task<IAuthorizedUser> GetUserByUserIdAndPassword(string userName, string password)
		{
			if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");
			if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
			return Task.Run(() =>
				{
					_log.Info(string.Format("Context.Request.GetUserName() user '{0}'", userName));
					var user = GetUserByEmailAndPassword(userName,password);
					return user.MapToIAuthorizedUser();
				});
		}

		public Task<string[]> GetRolesForUser(IAuthorizedUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.Run(() =>
			{
				_log.Info(string.Format("Roles user '{0}'", user.UserId));
				var foundUser = _generalUnitOfWork.Users.FirstOrDefault(x => x.Email == user.UserId);
				return foundUser != null ? foundUser.Roles.Select(x => x.Name).ToArray() : new string[0];
			});
		}

		public Task UpdateUserLastActivityDate(IAuthorizedUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.Run(() =>
			{
				_log.Info(string.Format("Roles user '{0}'", user.UserId));
				var userFound = _generalUnitOfWork.Users.FirstOrDefault(x => x.Email == user.UserId);
				if (userFound != null)
				{
					userFound.LastLoginDate = DateTime.Now;
					_generalUnitOfWork.Users.Update(userFound);
				}
			});
		}

		#endregion
	}
}