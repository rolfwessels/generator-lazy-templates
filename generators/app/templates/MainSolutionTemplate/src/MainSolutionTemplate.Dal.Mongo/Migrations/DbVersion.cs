using System;
using MainSolutionTemplate.Dal.Models.Interfaces;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
	public class DbVersion : IBaseDalModelWithId
	{
		public DbVersion()
		{
			CreateDate = DateTime.Now;
			UpdateDate = DateTime.Now;
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Version { get; set; }

		#region Implementation of IBaseDalModel

		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }

		#endregion
	}
}