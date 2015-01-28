namespace MainSolutionTemplate.OAuth2.Interfaces
{
	public interface IAuthorizedUser
	{
		string UserId { get; set; }
		string DisplayName { get; set; }
	}
}