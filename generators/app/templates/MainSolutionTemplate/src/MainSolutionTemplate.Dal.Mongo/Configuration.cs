using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Mongo.Migrations;
using MainSolutionTemplate.Dal.Mongo.Migrations.Versions;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
	public class Configuration
	{
	    private static readonly object _locker = new object();
		private static Configuration _instance;
	    private MongoMappers _mongoMappers;
	    private readonly IMigration[] _updates;
	    private bool _hasRun;
	    private Task _update;

	    protected Configuration()
	    {
	        _updates = new IMigration[] {
                new MigrateInitialize()
            };
	    }

	    public Task Update(IMongoDatabase db)
	    {
	        lock (_instance)
	        {
	            if (_update == null)
	            {
	                _mongoMappers = new MongoMappers();
	                _mongoMappers.InitializeMappers();
	                var versionUpdater = new VersionUpdater(_updates);
	                _update = versionUpdater.Update(db);
	            }
	        }
            return _update;
	    }

	    #region Instance

	    public static Configuration Instance()
	    {
	        if (_instance == null)
	            lock (_locker)
	            {
	                if (_instance == null)
	                {
	                    _instance = new Configuration();
	                }
	            }
	        return _instance;
	    }

	    #endregion

	}
}