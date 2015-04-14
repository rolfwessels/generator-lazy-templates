using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.OAuth2.Dal.DefaultValues
{
	public class SampleOAuthDataManager : IOAuthDataManager
	{
		private readonly List<DefaultAuthUser> _defaultAuthUsers;
		private readonly List<IRefreshToken> _refreshTokens;


		public SampleOAuthDataManager()
			: this(new List<DefaultAuthUser>()
				{
					new DefaultAuthUser("admin", "Admin authorizedUser", "admin!", new[] {"Admin"}),
					new DefaultAuthUser("guest", "Guest authorizedUser", "guest!", new[] {"Guest"})
				})
		{
		}

		public SampleOAuthDataManager(List<DefaultAuthUser> defaultAuthUsers)
		{
			if (defaultAuthUsers == null) throw new ArgumentNullException("defaultAuthUsers");
			_refreshTokens =	new List<IRefreshToken>();
			_defaultAuthUsers =  defaultAuthUsers ;

		}

		#region Implementation of IOAuthDataManager

        public Task<IOAuthClient> GetApplication(string clientId)
        {
            return Task.FromResult(new DefaultIoAuthClient()
                {
                    Active = true,
                    AllowedOrigin = "*",
                    RefreshTokenLifeTime = TimeSpan.FromDays(1).TotalMinutes
                }
 as IOAuthClient);
        }

		public Task<IAuthorizedUser> GetUserByUserIdAndPassword(string userName, string password)
		{
			if (userName == null) throw new ArgumentNullException("userName");
			if (password == null) throw new ArgumentNullException("password");
			return Task.FromResult(_defaultAuthUsers.FirstOrDefault(x => x.UserId.ToLower().Equals(userName.ToLower()) && x.Password == password) as IAuthorizedUser);
		}

		public Task<string[]> GetRolesForUser(IAuthorizedUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.FromResult(_defaultAuthUsers.Where(x => x.UserId == user.UserId).Select(x => x.Roles).FirstOrDefault());
		}

		public Task UpdateUserLastActivityDate(IAuthorizedUser user)
		{
			DefaultAuthUser defaultAuthUser = _defaultAuthUsers.FirstOrDefault(x => x.UserId == user.UserId);
			if (defaultAuthUser != null) defaultAuthUser.LastLoggedIn = DateTime.Now;
			return Task.FromResult(true);
		}

		public IRefreshToken CreateRefresherToken()
		{
			return new DefaultRefreshToken();
		}
		
		public Task SaveRefreshToken(IRefreshToken token)
		{
			_refreshTokens.Add(token);
			return Task.FromResult(token);
		}

		public Task<IRefreshToken> GetRefreshToken(string hashedTokenId)
		{
			return Task.FromResult(_refreshTokens.FirstOrDefault(x => x.Id == hashedTokenId));
		}

		public Task DeleteRefreshToken(string hashedTokenId)
		{
			return Task.FromResult(_refreshTokens.RemoveAll(x => x.Id == hashedTokenId));
		}

		#endregion
	}
}