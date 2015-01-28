using System;
using System.Collections.Generic;
using System.Linq;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
	public class MongoGeneralUnitOfWork : IGeneralUnitOfWork
	{
		private string _connectionString;
		private readonly string _databaseName;
		private readonly MongoClient _client;
		private readonly MongoServer _server;
		private MongoDatabase _db;


		public MongoGeneralUnitOfWork()
			: this("mongodb://localhost/MainSolutionTemplate_Develop")
		{
		}

		public MongoGeneralUnitOfWork(string connectionString)  
		{
			
			_connectionString = connectionString;
			_databaseName = new Uri(connectionString).Segments.Skip(1).FirstOrDefault()??"MainSolutionTemplate_Develop";

			_client = new MongoClient(connectionString);
			_server = _client.GetServer();
			_db = _server.GetDatabase(_databaseName);

			Users = new MongoRepository<User>(_db);
			Roles = new MongoRepository<Role>(_db);
			Applications = new MongoRepository<Application>(_db);
		}

		#region Implementation of IUnitOfWork

		public void Commit()
		{
			
		}

		public void Rollback()
		{
			
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			
		}

		#endregion

		#region Implementation of IGeneralUnitOfWork

		public IRepository<User> Users { get; private set; }
		public IRepository<Role> Roles { get; private set; }
		public IRepository<Application> Applications { get; private set; }

		#endregion
	}

}
