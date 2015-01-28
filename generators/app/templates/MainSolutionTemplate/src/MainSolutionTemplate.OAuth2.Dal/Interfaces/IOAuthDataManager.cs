using System.Threading.Tasks;

namespace MainSolutionTemplate.OAuth2.Dal.Interfaces
{
	public interface IOAuthDataManager : IRefreshTokenManager
	{
		Task<IClient> GetClient(string clientId);
		Task<IAuthorizedUser> GetUserByUserNameAndPassword(string userName, string password);
		Task<string[]> GetRolesForUser(IAuthorizedUser user);
		Task UpdateUserLastActivityDate(IAuthorizedUser user);
	}
}