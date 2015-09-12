using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MainSolutionTemplate.Dal.Models.Interfaces;

namespace MainSolutionTemplate.Dal.Persistance
{
	public interface IRepository<T> : IQueryable<T> where T : IBaseDalModel
	{
		T Add(T entity);
		IEnumerable<T> AddRange(IEnumerable<T> entities);
	    T Update(Expression<Func<T, bool>> filter, T entity);
	    bool Remove(Expression<Func<T, bool>> filter);
	}
}