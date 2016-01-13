using System;
using System.Collections.Generic;
using System.Linq;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
	public class MongoGeneralUnitOfWork : IGeneralUnitOfWork
	{
	    public MongoGeneralUnitOfWork(IMongoDatabase database)
	    {
            Configuration.Initialize(database);
            Users = new MongoRepository<User>(database);
            Applications = new MongoRepository<Application>(database);
            Projects = new MongoRepository<Project>(database);

	    }

		#region Implementation of IDisposable

		public void Dispose()
		{
			
		}

		#endregion

		#region Implementation of IGeneralUnitOfWork

		public IRepository<User> Users { get; private set; }
		public IRepository<Application> Applications { get; private set; }
	    public IRepository<Project> Projects { get; private set; }
	   
	    #endregion

        public IEnumerable<KeyValuePair<string, DataCounter>> Stats
        {
            get
            {
                var counters = new Object[] { Users,  Applications, Projects }.Cast<IHasDataCounter>();
                return counters.Select(x => new KeyValuePair<string, DataCounter>(x.GetType().GetGenericTypeDefinition().Name, x.DataCounter));
            }
        }
	}

}
