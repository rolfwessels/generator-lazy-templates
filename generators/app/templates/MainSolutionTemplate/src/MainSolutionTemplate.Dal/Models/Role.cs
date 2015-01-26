using System.Collections.Generic;

namespace MainSolutionTemplate.Dal.Models
{
	public class Role : BaseDalModelWithId
	{
		public string Name { get; set; }

		public virtual List<User> Users { get; set; }
	}
}