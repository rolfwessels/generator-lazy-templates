using System.Collections.Generic;
using System.Linq;
using MainSolutionTemplate.Dal.Models.Interfaces;

namespace MainSolutionTemplate.Dal.Persistance
{
	public interface IRepository<T> : IQueryable<T> where T : IBaseDalModel
	{
		T Add(T entity);
		IEnumerable<T> AddRange(IEnumerable<T> entities);
		bool Remove(T entity);
		T Update(T entity);
	}
}