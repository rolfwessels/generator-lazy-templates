﻿using System;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;

/**
 *  Note that this class can be optimised for EF. Currently it does not follow the full unit of work pattern.
 *  It has been designed this way as a easy introduction to EF an mongo working together
 *  Look at adding some transactions or moving the save SaveChanges into this class to get closer to unit of work pattern * 
 */
namespace MainSolutionTemplate.Dal.Ef
{

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
	public class EfGeneralUnitOfWork : IGeneralUnitOfWork
	{
		private readonly string _connectionStringName;
		private Lazy<GeneralDbContext> _context;

		public EfGeneralUnitOfWork() : this("MainSolutionTemplateContext")
		{
		}

		public EfGeneralUnitOfWork(string connectionStringName)
		{
			_connectionStringName = connectionStringName;
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
			_context = new Lazy<GeneralDbContext>(() => new GeneralDbContext(_connectionStringName));
			Users = new EfRepository<User>(_context.Value.Users, _context.Value);
			Roles = new EfRepository<Role>(_context.Value.Roles, _context.Value);
			Applications = new EfRepository<Application>(_context.Value.Applications, _context.Value);
		}

		#endregion

		#region Implementation of IDisposable

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
		public virtual void Dispose()
		{
			if (_context.IsValueCreated) _context.Value.Dispose();
		}

		#endregion

		#region Implementation of IGeneralUnitOfWork

		public IRepository<User> Users { get; private set; }
		public IRepository<Role> Roles { get; private set; }
		public IRepository<Application> Applications { get; private set; }

		#endregion
	}
}