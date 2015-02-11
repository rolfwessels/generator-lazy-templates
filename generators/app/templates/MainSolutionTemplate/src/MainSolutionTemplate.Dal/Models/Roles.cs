namespace MainSolutionTemplate.Dal.Models
{
	public class Roles
	{
		public static Role Admin {
			get
			{
				return new Role() { Name = "Admin"};
			}
		}
	}
}