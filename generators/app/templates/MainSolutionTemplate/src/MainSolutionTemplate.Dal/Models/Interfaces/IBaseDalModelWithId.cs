using System;

namespace MainSolutionTemplate.Dal.Models.Interfaces
{
	public interface IBaseDalModelWithId : IBaseDalModel
	{
		Guid Id { get; set; }
	}
}