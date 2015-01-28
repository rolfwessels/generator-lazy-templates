using System;
using System.Collections.Generic;

namespace MainSolutionTemplate.Dal.Models
{
	public class User : BaseDalModelWithId
	{
		public User()
		{
			Roles  = new List<Role>();
		}

		public string Name { get; set; }
		public string Email { get; set; }
		public string HashedPassword { get; set; }
		public virtual List<Role> Roles { get; private set; }
		public DateTime? LastLoginDate { get; set; }
	}
}