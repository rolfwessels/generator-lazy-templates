using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
    public class VersionUpdator
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Mutex _dbUpdateLocker = new Mutex();
        private readonly IMigration[] _updates;


        public VersionUpdator(IMigration[] updates)
        {
            _updates = updates;
        }

        public async Task Update(IMongoDatabase db)
        {
            if (_dbUpdateLocker.WaitOne(TimeSpan.FromSeconds(30)))
            {
                var repository = new MongoRepository<DbVersion>(db);
                List<DbVersion> versions = await repository.Find();
                for (int i = 0; i < _updates.Length; i++)
                {
                    IMigration migrateInitialize = _updates[i];
                    await EnsureThatVersionDoesNotExistThenUpdate(versions, i, migrateInitialize, repository, db);
                }
            }
        }

        #region Private Methods

        private async Task EnsureThatVersionDoesNotExistThenUpdate(IEnumerable<DbVersion> versions, int i, IMigration migrateInitialize, MongoRepository<DbVersion> repository, IMongoDatabase db)
        {
            DbVersion version = versions.FirstOrDefault(x => x.Id == i);
            if (version == null)
            {
                RunTheUpdate(migrateInitialize, db);
                var dbVersion1 = new DbVersion {Id = i, Name = migrateInitialize.GetType().Name};
                await repository.Add(dbVersion1);
            }
        }

        private void RunTheUpdate(IMigration migrateInitialize, IMongoDatabase db)
        {
            _log.Info(string.Format("Starting {0} db update", migrateInitialize.GetType().Name));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            migrateInitialize.Update(db);
            stopwatch.Stop();
            _log.Info(string.Format("Done {0} in {1}ms", migrateInitialize.GetType().Name, stopwatch.ElapsedMilliseconds));
        }

        #endregion
    }
}