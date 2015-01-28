using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MainSolutionTemplate.Dal.Mongo
{
	public class MongoRepository<T> : IRepository<T> where T : IBaseDalModel
	{
		private readonly MongoDatabase _db;
		private readonly MongoCollection<T> _mongoCollection;

		public MongoRepository(MongoDatabase db)
		{
			_db = db;
			_mongoCollection = _db.GetCollection<T>(typeof(T).Name);
		}

		#region Implementation of IRepository<T>

		public IQueryable<T> All {
			get { return _mongoCollection.AsQueryable(); }
		}

		public T Add(T entity)
		{
			entity.CreateDate = DateTime.Now;
			entity.UpdateDate = DateTime.Now;
			_mongoCollection.Save(entity);
			return entity;
		}

		public IEnumerable<T> AddRange(IEnumerable<T> entities)
		{
			return entities.Select(Add);
		}

		public bool Remove(T entity)
		{
			var baseDalModelWithId = entity as IBaseDalModelWithId;
			if (baseDalModelWithId != null)
			{
				var query = new QueryDocument("_id", BsonValue.Create(baseDalModelWithId.Id));
				WriteConcernResult writeConcernResult = _mongoCollection.Remove(query);
				return writeConcernResult.Ok;
			}
			throw new Exception(string.Format("Entity {0} must have known id to be removed", typeof(T).Name));
		}

		public T Update(T entity)
		{
			entity.UpdateDate = DateTime.Now;
			_mongoCollection.Save(entity);
			return entity;
		}

		public int RemoveRange(IEnumerable<T> entities)
		{
			return entities.Where(Remove).Count();
		}

		#endregion

		#region Implementation of IEnumerable

		public IEnumerator<T> GetEnumerator()
		{
			return _mongoCollection.AsQueryable().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of IQueryable

		public Expression Expression {
			get { return _mongoCollection.AsQueryable().Expression; }
		}
		public Type ElementType {
			get { return _mongoCollection.AsQueryable().ElementType; }
		}
		public IQueryProvider Provider {
			get { return _mongoCollection.AsQueryable().Provider; }
		}

		#endregion
	}
}