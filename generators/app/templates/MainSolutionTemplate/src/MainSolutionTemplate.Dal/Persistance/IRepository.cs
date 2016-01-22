using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Models.Interfaces;

namespace MainSolutionTemplate.Dal.Persistance
{
	public interface IRepository<T> where T : IBaseDalModel
	{
	    IQueryable<T> Query();
	    Task<T> Add(T entity);
	    Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
	    Task<T> Update(Expression<Func<T, bool>> filter, T entity);
        Task<long> Update<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> update, TType value) where TType : class;
	    Task<bool> Remove(Expression<Func<T, bool>> filter);
	    Task<List<T>> Find(Expression<Func<T, bool>> filter);
	    Task<T> FindOne(Expression<Func<T, bool>> filter);
	    Task<long> Count();
	    Task<long> Count(Expression<Func<T, bool>> filter);
	    
	}
}