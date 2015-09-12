using System;
using System.Collections.Generic;
using System.Linq;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Mongo.Migrations;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
	public class MongoGeneralUnitOfWork : IGeneralUnitOfWork
	{

	    private IMongoDatabase _database;


	    public MongoGeneralUnitOfWork(IMongoDatabase database)
	    {
	        _database = database;
            Configuration.Initialize(_database);
            Users = new MongoRepository<User>(_database);
            Roles = new MongoRepository<Role>(_database);
            Applications = new MongoRepository<Application>(_database);
            Projects = new MongoRepository<Project>(_database);
	    }

	    #region Implementation of IUnitOfWork

		public void Commit()
		{
			
		}

		public void Rollback()
		{
			
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			
		}

		#endregion

		#region Implementation of IGeneralUnitOfWork

		public IRepository<User> Users { get; private set; }
		public IRepository<Role> Roles { get; private set; }
		public IRepository<Application> Applications { get; private set; }
	    public IRepository<Project> Projects { get; private set; }

	    #endregion
	}

}
