using System.Threading.Tasks;

namespace MainSolutionTemplate.OAuth2.Dal.Interfaces
{
	public interface IOAuthDataManager 
	{
		Task<IOAuthClient> GetApplication(string clientId);
		Task<IAuthorizedUser> GetUserByUserIdAndPassword(string userName, string password);
		Task<string[]> GetRolesForUser(IAuthorizedUser user);
		Task UpdateUserLastActivityDate(IAuthorizedUser user);
	}
}