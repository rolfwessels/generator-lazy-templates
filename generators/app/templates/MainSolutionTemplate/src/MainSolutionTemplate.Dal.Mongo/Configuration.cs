using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Mongo.Migrations;
using MainSolutionTemplate.Dal.Mongo.Migrations.Versions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
	public class Configuration
	{
		private static readonly object _locker = new object();
		private static Configuration _instance;
	    private readonly MongoMappers _mongoMappers;

	    protected Configuration(IMongoDatabase db)
		{
            _mongoMappers = new MongoMappers();
            _mongoMappers.InitializeMappers();
            var updates = new IMigration[] {
                new MigrateInitialize()
            };
	        var versionUpdator = new VersionUpdator(updates);
            versionUpdator.Update(db).Wait();
	        
		}

	    #region Initialize

		public static void Initialize(IMongoDatabase db)
		{
            if (_instance != null) return;
			lock (_locker)
			{
                if (_instance == null)
				{
					_instance = new Configuration(db);
				}
			}
		}

		#endregion

	}
}