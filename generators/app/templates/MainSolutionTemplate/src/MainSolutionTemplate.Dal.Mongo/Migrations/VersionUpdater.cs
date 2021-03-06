﻿using System;
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
    public class VersionUpdater
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly IMigration[] _updates;
        private readonly Mutex _resetEvent = new Mutex(false, @"global/MainSolutionTemplate_VersionUpdater");
        
        public VersionUpdater(IMigration[] updates)
        {

            _updates = updates;
        }

        public Task Update(IMongoDatabase db)
        {
            var task = Task.Run(() =>
            {
                if (_resetEvent.WaitOne(TimeSpan.FromSeconds(5)))
                {
                    
                    var repository = new MongoRepository<DbVersion>(db);
                    List<DbVersion> versions = repository.Find().Result;
                    _log.Info(string.Format("Found {0} database updates in database and {1} in code", versions.Count, _updates.Length));
                    for (int i = 0; i < _updates.Length; i++)
                    {   
                        IMigration migrateInitialize = _updates[i];
                        EnsureThatVersionDoesNotExistThenUpdate(versions, i, migrateInitialize, repository, db).Wait();
                    }
                    _log.Info("Done");
                    
                    _resetEvent.ReleaseMutex();
                }

            });
            task.ConfigureAwait(false);
            return task;
        }

        #region Private Methods

        private async Task EnsureThatVersionDoesNotExistThenUpdate(IEnumerable<DbVersion> versions, int i, IMigration migrateInitialize, MongoRepository<DbVersion> repository, IMongoDatabase db)
        {
            DbVersion version = versions.FirstOrDefault(x => x.Id == i);
            if (version == null)
            {
                _log.Info(string.Format("Running version update {0}", migrateInitialize.GetType().Name));
                await RunTheUpdate(migrateInitialize, db);
                var dbVersion1 = new DbVersion {Id = i, Name = migrateInitialize.GetType().Name};
                await repository.Add(dbVersion1);
            }
        }

        private async Task RunTheUpdate(IMigration migrateInitialize, IMongoDatabase db)
        {
            _log.Info(string.Format("Starting {0} db update", migrateInitialize.GetType().Name));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await migrateInitialize.Update(db);
            stopwatch.Stop();
            _log.Info(string.Format("Done {0} in {1}ms", migrateInitialize.GetType().Name, stopwatch.ElapsedMilliseconds));
        }

        #endregion
    }
}