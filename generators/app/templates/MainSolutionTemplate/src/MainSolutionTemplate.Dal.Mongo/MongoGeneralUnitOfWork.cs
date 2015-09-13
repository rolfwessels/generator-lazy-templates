using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Mongo.Migrations;
using MainSolutionTemplate.Dal.Persistance;
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
	    public IEnumerable<KeyValuePair<string, DataCounter>> Stats {
	        get
	        {
	            dynamic expandoObject = new ExpandoObject();
                var foo = new Object[] { Users, Roles, Applications, Projects }.Cast<IHasDataCounter>();


                return foo.Select(x => new KeyValuePair<string, DataCounter>(x.GetType().GetGenericTypeDefinition().Name, x.DataCounter));
	        }
	    }

	    #endregion
	}

}
