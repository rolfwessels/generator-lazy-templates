using System.Collections.Generic;
using System.Linq;

namespace MainSolutionTemplate.Dal.Persistance
{
	public interface IRepository<T> 
	{
		IQueryable<T> All { get; }
		T Add(T entity);
		IEnumerable<T> AddRange(IEnumerable<T> entities);
		T Remove(T entity);
		IEnumerable<T> RemoveRange(IEnumerable<T> entities);	
	}
}