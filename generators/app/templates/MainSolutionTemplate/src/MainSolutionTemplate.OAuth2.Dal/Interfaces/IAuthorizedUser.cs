namespace MainSolutionTemplate.OAuth2.Dal.Interfaces
{
	public interface IAuthorizedUser
	{
		string UserId { get; set; }
		string DisplayName { get; set; }
	}
}