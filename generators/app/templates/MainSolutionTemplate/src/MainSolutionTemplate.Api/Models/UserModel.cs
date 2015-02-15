using System;

namespace MainSolutionTemplate.Api.Models
{
	public class UserModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public DateTime? LastLoginDate { get; set; }

		public override string ToString()
		{
			return string.Format("Id: {0}, Email: {1}, Name: {2}", Id, Email, Name);
		}
	}

	
}