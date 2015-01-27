using System;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Persistance
{
	public interface IGeneralUnitOfWork : IUnitOfWork, IDisposable
	{
		IRepository<User> Users { get;  }
		IRepository<Role> Roles { get;  }
	}
}