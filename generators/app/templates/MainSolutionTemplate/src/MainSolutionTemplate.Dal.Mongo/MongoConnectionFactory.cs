using System;
using System.Linq;
using System.Reflection;
using log4net;
using MainSolutionTemplate.Dal.Mongo.Properties;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class MongoConnectionFactory : IGeneralUnitOfWorkFactory
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _connectionString;
        private readonly string _databaseName;
        private Lazy<IGeneralUnitOfWork> _singleConnection;

        public MongoConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
            _databaseName = new Uri(_connectionString).Segments.Skip(1).FirstOrDefault() ?? "MainSolutionTemplate";
            _singleConnection = new Lazy<IGeneralUnitOfWork>(GeneralUnitOfWork);
        }

        public static IGeneralUnitOfWork New
        {
            get { return new MongoConnectionFactory(Settings.Default.Connection).GetConnection(); }
        }

        public string DatabaseName
        {
            get { return _databaseName; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        #region IGeneralUnitOfWorkFactory Members

        public IGeneralUnitOfWork GetConnection()
        {
            return _singleConnection.Value;
        }

        private IGeneralUnitOfWork GeneralUnitOfWork()
        {
            _log.Info("Create new connection to " + _connectionString);
            IMongoDatabase database = DatabaseOnly();
            return new MongoGeneralUnitOfWork(database);
        }

        #endregion

        public IMongoDatabase DatabaseOnly()
        {
            IMongoClient client = ClientOnly();
            IMongoDatabase database = client.GetDatabase(_databaseName);
            return database;
        }

        #region Private Methods

        private IMongoClient ClientOnly()
        {
            return new MongoClient(_connectionString);
        }

        #endregion
    }
}