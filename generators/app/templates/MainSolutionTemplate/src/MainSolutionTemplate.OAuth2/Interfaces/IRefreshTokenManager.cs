using System.Threading.Tasks;

namespace MainSolutionTemplate.OAuth2.Interfaces
{
	public interface IRefreshTokenManager
	{
		IRefreshToken CreateRefresherToken();
		Task SaveRefreshToken(IRefreshToken token);
		Task<IRefreshToken> GetRefreshToken(string hashedTokenId);
		Task DeleteRefreshToken(string hashedTokenId);
	}
}