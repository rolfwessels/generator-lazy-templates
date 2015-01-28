namespace MainSolutionTemplate.OAuth2.Interfaces
{
	public interface IOAuthSecurity
	{
		string GetHash(string clientSecret);
	}
}