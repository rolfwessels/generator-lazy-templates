using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
	public class Configuration
	{
		private static readonly object _locker = new object();
		private static Configuration _instance;

		protected Configuration(IMongoDatabase db)
		{
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