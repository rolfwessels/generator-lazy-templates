using System.Collections.Generic;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Dal.Models
{
	public class Role
	{
		public string Name { get; set; }
		public List<Activity> Activities { get; set; }
	}
}