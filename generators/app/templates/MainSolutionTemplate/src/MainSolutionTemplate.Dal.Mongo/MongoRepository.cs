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
		private readonly IMongoDatabase _db;
		private readonly IMongoCollection<T> _mongoCollection;
	    private FilterDefinition<T> _keyFilter;
	    private IMongoCollection<BsonDocument> _mongoCollectionUnTyped;

	    public MongoRepository(IMongoDatabase db)
		{
			_db = db;
			_mongoCollection = _db.GetCollection<T>(typeof(T).Name);
            _mongoCollectionUnTyped = _db.GetCollection<BsonDocument>(typeof(T).Name);
            
		}

		#region Implementation of IRepository<T>


		public T Add(T entity)
		{
			entity.CreateDate = DateTime.Now;
			entity.UpdateDate = DateTime.Now;
		    _mongoCollection.InsertOneAsync(entity).Wait();
		    return entity;
		}

		public IEnumerable<T> AddRange(IEnumerable<T> entities)
		{
		    var enumerable = entities as IList<T> ?? entities.ToList();
		    foreach (var entity in enumerable)
		    {
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
		    }
		    _mongoCollection.InsertManyAsync(enumerable);
		    return enumerable;
		}

	    public bool Remove(Expression<Func<T, bool>> filter)
	    {
            var deleteResult = _mongoCollection.DeleteOneAsync(filter).Result;
            return deleteResult.DeletedCount > 0;
	    }


	    public T Update(Expression<Func<T, bool>> filter, T entity)
		{
			entity.UpdateDate = DateTime.Now; 
	        _mongoCollection.ReplaceOneAsync(Builders<T>.Filter.Where(filter),entity).Wait();
			return entity;
		}

		#endregion

	    #region Implementation of IEnumerable

	    public IEnumerator<T> GetEnumerator()
	    {
	        throw new NotImplementedException();
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }

	    #endregion

	    #region Implementation of IQueryable

	    public Expression Expression { get; private set; }
	    public Type ElementType { get; private set; }
	    public IQueryProvider Provider { get; private set; }

	    #endregion
	}
}