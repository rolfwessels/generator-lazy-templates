using System;
using System.Collections.Generic;
using MainSolutionTemplate.Dal.Models.Reference;

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
		public List<string> Roles { get; private set; }
		public DateTime? LastLoginDate { get; set; }
	    public ProjectReference DefaultProject { get; set; }
	    
	    public override string ToString()
		{
			return string.Format("Email: {0}, Name: {1}", Email, Name);
		}
	}
}