using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using log4net;
using MainSolutionTemplate.Dal.Mongo.Migrations;
using MainSolutionTemplate.Dal.Mongo.Migrations.Versions;
using MainSolutionTemplate.Dal.Mongo.Properties;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests
{
    [TestFixture]
    public class VersionUpdaterTests
    {

        
        private MongoConnectionFactory _mongoConnectionFactory;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IMigration[] _updates;

        #region Setup/Teardown

        public void Setup()
        {
            _updates = new IMigration[] {
                new MigrateInitialize()
            };
            _mongoConnectionFactory = new MongoConnectionFactory(Settings.Default.Connection);
            
        }

        #endregion
        
        [Test]
        public async Task MultipleThreadsOnOne_GivenUpdate_ShouldBlockAllOtherThreads()
        {
            _log.Info("Run");
            // arrange
            Setup();
            var mongoDatabase = _mongoConnectionFactory.DatabaseOnly();
            var tasks = new List<Thread>();
            for (int i = 0; i < 2; i++)
            {
                tasks.Add(new Thread(() =>
                {
                    var versionUpdater = new VersionUpdater(_updates);
                    versionUpdater.Update(mongoDatabase).Wait();
                }));
            }
            
            // action
            _log.Info("Starting all");
            foreach (var thread in tasks)
            {
                thread.Start();
            }
            // assert
            foreach (var thread in tasks)
            {
                thread.Join();
            }
            _log.Info("Done all");
        }


         
    }

}