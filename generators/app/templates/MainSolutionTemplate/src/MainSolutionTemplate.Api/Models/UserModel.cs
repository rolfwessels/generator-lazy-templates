using System;

namespace MainSolutionTemplate.Api.Models
{
	public class UserModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string HashedPassword { get; set; }
		public DateTime? LastLoginDate { get; set; }
	}

	
}