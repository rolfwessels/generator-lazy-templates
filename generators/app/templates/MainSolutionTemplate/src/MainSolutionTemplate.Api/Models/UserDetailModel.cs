using System;

namespace MainSolutionTemplate.Api.Models
{
	public class UserDetailModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public DateTime? LastLoginDate { get; set; }

		public override string ToString()
		{
			return string.Format("Email: {0}, LastLoginDate: {1}, Name: {2}", Email, LastLoginDate, Name);
		}
	}
}