using System;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Dal.Ef
{
	public class EfGeneralUnitOfWork : IGeneralUnitOfWork 
	{
		private readonly string _connnectionStringName;
		private Lazy<GeneralDbContext> _context;


		public EfGeneralUnitOfWork() : this("MainSolutionTemplateContext")
		{

		}

		public EfGeneralUnitOfWork(string connnectionStringName)
		{
			_connnectionStringName = connnectionStringName;
			ReCreateContext();
			
		}

		
		#region Implementation of IUnitOfWork


		public void Rollback()
		{
			ReCreateContext();
		}

		#endregion

		

		#region Private Methods

		private void ReCreateContext()
		{
			if (_context != null && _context.IsValueCreated) _context.Value.Dispose();
			_context = new Lazy<GeneralDbContext>(() => new GeneralDbContext(_connnectionStringName));
			Users = new RepositoryWrapper<User>(_context.Value.UsersSet, _context.Value);
			Roles = new RepositoryWrapper<Role>(_context.Value.RoleSet, _context.Value);
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			if (_context.IsValueCreated) _context.Value.Dispose();
		}

		#endregion

		#region Implementation of IGeneralUnitOfWork

		public IRepository<User> Users { get; private set; }
		public IRepository<Role> Roles { get; private set; }

		#endregion
	}
}