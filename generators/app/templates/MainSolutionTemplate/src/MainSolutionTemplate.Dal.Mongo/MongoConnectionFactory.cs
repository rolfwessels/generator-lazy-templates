using System;
using System.Linq;
using System.Reflection;
using log4net;
using MainSolutionTemplate.Dal.Mongo.Properties;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class MongoConnectionFactory
    {
        private string _connectionString;
        private string _databaseName;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MongoConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
            _databaseName = new Uri(_connectionString).Segments.Skip(1).FirstOrDefault() ?? "MainSolutionTemplate";

        }

        public static IGeneralUnitOfWork New {
            get { return new MongoConnectionFactory(Settings.Default.Connection).GetConnection(); }
        }

        public IGeneralUnitOfWork GetConnection()
        {
            _log.Info("Create new connection to " + _connectionString);
            var database = DatabaseOnly();
            return new MongoGeneralUnitOfWork(database);
        }

        public IMongoDatabase DatabaseOnly()
        {
            var client = ClientOnly();
            var database = client.GetDatabase(_databaseName);
            return database;
        }

        private IMongoClient ClientOnly()
        {
            return new MongoClient(_connectionString);
        }

        public string DatabaseName
        {
            get { return _databaseName; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

    }

   
}