using System.Threading.Tasks;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.Core.Managers
{
	public class SystemManagerFacade : ISystemManagerFacade
	{
		private readonly IOAuthDataManager _oauthManager;

		public SystemManagerFacade(IOAuthDataManager oauthManager)
		{
			_oauthManager = oauthManager;
		}

		public IRefreshToken CreateRefresherToken()
		{
			return _oauthManager.CreateRefresherToken();
		}

		public Task SaveRefreshToken(IRefreshToken token)
		{
			return _oauthManager.SaveRefreshToken(token);
		}

		public Task<IRefreshToken> GetRefreshToken(string hashedTokenId)
		{
			return _oauthManager.GetRefreshToken(hashedTokenId);
		}

		public Task DeleteRefreshToken(string hashedTokenId)
		{
			return _oauthManager.DeleteRefreshToken(hashedTokenId);
		}

		public Task<IOAuthClient> GetApplication(string clientId)
		{
			return _oauthManager.GetApplication(clientId);
		}

		public Task<IAuthorizedUser> GetUserByUserIdAndPassword(string userName, string password)
		{
			return _oauthManager.GetUserByUserIdAndPassword(userName, password);
		}

		public Task<string[]> GetRolesForUser(IAuthorizedUser user)
		{
			return _oauthManager.GetRolesForUser(user);
		}

		public Task UpdateUserLastActivityDate(IAuthorizedUser user)
		{
			return _oauthManager.UpdateUserLastActivityDate(user);
		}
	}
}