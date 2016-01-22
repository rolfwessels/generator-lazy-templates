using System;
using MainSolutionTemplate.Dal.Models.Interfaces;

namespace MainSolutionTemplate.Dal.Models
{
	public abstract class BaseDalModel : IBaseDalModel
	{
		public BaseDalModel()
		{
			CreateDate = DateTime.Now;
			UpdateDate = DateTime.Now;
		}

		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
	}
}