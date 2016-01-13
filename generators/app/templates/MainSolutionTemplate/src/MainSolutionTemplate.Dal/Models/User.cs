using System;
using System.Collections.Generic;

namespace MainSolutionTemplate.Dal.Models
{
	public class User : BaseDalModelWithId
	{
		public User()
		{
            Roles = new List<string>();
		}

		public string Name { get; set; }
		public string Email { get; set; }
		public string HashedPassword { get; set; }
		public virtual List<string> Roles { get; private set; }
		public DateTime? LastLoginDate { get; set; }

		public override string ToString()
		{
			return string.Format("Email: {0}, Name: {1}", Email, Name);
		}
	}
}