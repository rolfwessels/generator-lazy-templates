using System;
using System.Threading.Tasks;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using System.Linq;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManager : IOAuthDataManager
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
			return Task.Run(() => _generalUnitOfWork.Applications.FirstOrDefault(x => x.ClientId == clientId).MapToIOAuthClient());
		}

		public Task<IAuthorizedUser> GetUserByUserIdAndPassword(string userName, string password)
		{
			return Task.Run(() =>
				{
					_log.Info(string.Format("Login user '{0}'", userName));
					var user = GetUserByEmailAndPassword(userName,password);
					return user.MapToIAuthorizedUser();
				});
		}

		public Task<string[]> GetRolesForUser(IAuthorizedUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.Run(() =>
			{
				var foundUser = _generalUnitOfWork.Users.FirstOrDefault(x => x.Email == user.UserId);
				return foundUser != null ? foundUser.Roles.Select(x => x.Name).ToArray() : new string[0];
			});
		}

		public Task UpdateUserLastActivityDate(IAuthorizedUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.Run(() =>
			{
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