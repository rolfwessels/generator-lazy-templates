using System;

namespace MainSolutionTemplate.Dal.Models
{
	public class BaseDalModel
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