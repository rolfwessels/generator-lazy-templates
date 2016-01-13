using System;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Persistance
{
	public interface IGeneralUnitOfWork : IDisposable
	{
		IRepository<User> Users { get;  }
		IRepository<Application> Applications { get; }
		IRepository<Project> Projects { get; }
	}
}