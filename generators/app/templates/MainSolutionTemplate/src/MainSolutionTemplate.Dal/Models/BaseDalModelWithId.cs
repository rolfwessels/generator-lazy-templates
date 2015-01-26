using System;

namespace MainSolutionTemplate.Dal.Models
{
	public class BaseDalModelWithId : BaseDalModel
	{
		public BaseDalModelWithId()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
	}
}