using System;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Dal.Ef
{
	public class GeneralUnitOfWork : IGeneralUnitOfWork 
	{
		private string _connnectionStringName;
		private Lazy<GeneralDbContext> _context;


		public GeneralUnitOfWork() : this("MainSolutionTemplateContext")
		{

		}

		public GeneralUnitOfWork(string connnectionStringName)
		{
			_connnectionStringName = connnectionStringName;
			ReCreateContext();
		}

		
		#region Implementation of IUnitOfWork

		public void Commit()
		{
			_context.Value.SaveChanges();
		}

		public void Rollback()
		{
			ReCreateContext();
		}

		#endregion

		#region Implementation of IGeneralUnitOfWork

		public IRepository<User> Users {
			get { return new RepositoryWrapper<User>(_context.Value.UsersSet); } 
		}
		public IRepository<Role> Roles {
			get { return new RepositoryWrapper<Role>(_context.Value.RoleSet); } 
		}

		#endregion

		#region Private Methods

		private void ReCreateContext()
		{
			if (_context != null && _context.IsValueCreated) _context.Value.Dispose();
			_context = new Lazy<GeneralDbContext>(() => new GeneralDbContext(_connnectionStringName)); 
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			if (_context.IsValueCreated) _context.Value.Dispose();
		}

		#endregion
	}
}