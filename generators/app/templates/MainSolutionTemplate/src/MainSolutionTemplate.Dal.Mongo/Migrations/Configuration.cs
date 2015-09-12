using System.Security.Cryptography.X509Certificates;
using MainSolutionTemplate.Dal.Models;
using MongoDB.Driver;
using System.Linq;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
	public class Configuration
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static Configuration _instance;

		protected Configuration(IMongoDatabase db)
		{
		    // todo: Rolf add mutex 
            // todo: Rolf Add versions to array 

			var mongoRepository = new MongoRepository<DbVersion>(db);
			var dbVersion = mongoRepository.FirstOrDefault() ?? new DbVersion() {Name = "General"};
			try
			{
				switch (dbVersion.Version)
				{
					case 0: new MigrateInitialize(db);
						dbVersion.Version++;
						break;
				}
			}
			finally 
			{
				mongoRepository.Update(x=>x.Id == dbVersion.Id,dbVersion);
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