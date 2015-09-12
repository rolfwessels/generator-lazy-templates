using System;
using System.Linq;
using MainSolutionTemplate.Dal.Mongo.Properties;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class MongoDbConnectionFactory
    {
        private string _connectionString;
        private string _databaseName;

        public MongoDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
            _databaseName = new Uri(_connectionString).Segments.Skip(1).FirstOrDefault() ?? "MainSolutionTemplate";

        }

        public static IGeneralUnitOfWork New {
            get { return new MongoDbConnectionFactory(Settings.Default.Connection).GetConnection(); }
        }

        public IGeneralUnitOfWork GetConnection()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);
            return new MongoGeneralUnitOfWork(database);
        }
    }

   
}