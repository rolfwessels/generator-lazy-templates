using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class MongoRepository<T> : IRepository<T> where T : IBaseDalModel
	{
        private readonly IMongoCollection<T> _mongoCollection;

        public MongoRepository(IMongoDatabase database)
		{
	        
	        _mongoCollection = database.GetCollection<T>(typeof(T).Name);
            
		}

		#region Implementation of IRepository<T>


		public async Task<T> Add(T entity)
		{
			entity.CreateDate = DateTime.Now;
			entity.UpdateDate = DateTime.Now;
		    await _mongoCollection.InsertOneAsync(entity);
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

	    public async Task<bool> Remove(Expression<Func<T, bool>> filter)
	    {
            var deleteResult = await _mongoCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
	    }


	    public async Task<T> Update(Expression<Func<T, bool>> filter, T entity)
		{
			entity.UpdateDate = DateTime.Now;
	        await _mongoCollection.ReplaceOneAsync(Builders<T>.Filter.Where(filter), entity);
			return entity;
		}

        public Task<List<T>> Find(Expression<Func<T, bool>> filter)
	    {
	        return _mongoCollection.Find(Builders<T>.Filter.Where(filter)).ToListAsync();
	    }

        public Task<T> FindOne(Expression<Func<T, bool>> filter)
	    {
            return _mongoCollection.Find(Builders<T>.Filter.Where(filter)).FirstOrDefaultAsync();
	    }


        public Task<long> Count()
        {
            return Count((x)=>true);
        }

        public Task<long> Count(Expression<Func<T, bool>> filter)
        {
            return _mongoCollection.CountAsync(Builders<T>.Filter.Where(filter));
        }

        #endregion
	}
}