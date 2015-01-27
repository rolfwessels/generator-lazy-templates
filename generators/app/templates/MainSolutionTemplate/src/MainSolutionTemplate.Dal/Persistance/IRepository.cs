using System.Collections.Generic;
using System.Linq;

namespace MainSolutionTemplate.Dal.Persistance
{
	public interface IRepository<T> : IQueryable<T>
	{
		T Add(T entity);
		IEnumerable<T> AddRange(IEnumerable<T> entities);
		bool Remove(T entity);
	}
}