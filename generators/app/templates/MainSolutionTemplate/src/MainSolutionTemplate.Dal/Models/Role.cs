using System.Collections.Generic;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Dal.Models
{
	public class Role : BaseDalModelWithId
	{
		public string Name { get; set; }
		public List<Activity> Activities { get; set; }
		public virtual List<User> Users { get; set; }
	}
}