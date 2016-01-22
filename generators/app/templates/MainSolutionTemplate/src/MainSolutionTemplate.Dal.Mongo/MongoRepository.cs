using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class MongoRepository<T> : IRepository<T> where T : IBaseDalModel
    {
        private readonly IMongoCollection<T> _mongoCollection;

        public MongoRepository(IMongoDatabase database)
        {
            _mongoCollection = database.GetCollection<T>(typeof (T).Name);
        }

        public IMongoCollection<T> Collection
        {
            get { return _mongoCollection; }
        }

        #region Implementation of IRepository<T>

        public IQueryable<T> Query()
        {
            return _mongoCollection.AsQueryable();
        }

        public async Task<T> Add(T entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            await _mongoCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
        {
            IList<T> enumerable = entities as IList<T> ?? entities.ToList();
            foreach (T entity in enumerable)
            {
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
            }
            await _mongoCollection.InsertManyAsync(enumerable);

            return enumerable;
        }


        public async Task<T> Update(Expression<Func<T, bool>> filter, T entity)
        {
            entity.UpdateDate = DateTime.Now;
            await _mongoCollection.ReplaceOneAsync(Builders<T>.Filter.Where(filter), entity);
            return entity;
        }

        public async Task<long> Update<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> update, TType value) where TType : class
        {
            var filterDefinition = Builders<T>.Filter.Where(filter);
            var updateDefinition = Builders<T>.Update
                .Set(update, value)
                .CurrentDate(x=>x.UpdateDate);
                
            var result = await _mongoCollection.UpdateManyAsync(filterDefinition, updateDefinition);
            return result.ModifiedCount;

        }

        public async Task<bool> Remove(Expression<Func<T, bool>> filter)
        {
            DeleteResult deleteResult = await _mongoCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
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
            return Count(x => true);
        }

        public Task<long> Count(Expression<Func<T, bool>> filter)
        {
            return _mongoCollection.CountAsync(Builders<T>.Filter.Where(filter));
        }

        #endregion

        public Task<List<T>> Find()
        {
            return Find(x => true);
        }
    }
}