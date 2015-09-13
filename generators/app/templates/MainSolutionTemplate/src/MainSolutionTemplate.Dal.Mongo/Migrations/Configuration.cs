using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using MainSolutionTemplate.Dal.Models;
using MongoDB.Driver;
using System.Linq;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
	public class Configuration
	{
        private static Mutex _mutex = new Mutex();
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static Configuration _instance;

		protected Configuration(IMongoDatabase db)
		{
            var updates = new IMigration[] {
                new MigrateInitialize()
            };
            
            if (_mutex.WaitOne(TimeSpan.FromSeconds(30)))
            {
                var versions = new MongoRepository<DbVersion>(db);
                for (int i = 0; i < updates.Length; i++)
                {
                    var migrateInitialize = updates [i];
                    var version = versions.FindOne(x => x.Id == i).Result;
                    if (version == null)
                    {
                        migrateInitialize.Update(db);
                        var dbVersion1 = new DbVersion() { Id = i, Name = migrateInitialize.GetType().Name };
                        versions.Add(dbVersion1);
                        
                    }
                }
            }
		}

		#region Initialize

		

		public static void Initialize(IMongoDatabase db)
		{
			if (_isInitialized) return;
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_instance = new Configuration(db);
					_isInitialized = true;
				}
			}
		}

		#endregion

	}
}