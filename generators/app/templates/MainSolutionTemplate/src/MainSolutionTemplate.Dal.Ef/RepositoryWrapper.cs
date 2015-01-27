using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Dal.Ef
{
	public class RepositoryWrapper<T> : IRepository<T> where T : class
	{
		private readonly DbSet<T> _usersSet;

		public RepositoryWrapper(DbSet<T> usersSet)
		{
			_usersSet = usersSet;
		}

		public IQueryable<T> All {
			get { return _usersSet; }
		}

		public T Add(T entity)
		{
			return _usersSet.Add(entity);
		}

		public IEnumerable<T> AddRange(IEnumerable<T> entities)
		{
			return _usersSet.AddRange(entities);
		}

		public T Remove(T entity)
		{
			return _usersSet.Remove(entity);
		}

		public IEnumerable<T> RemoveRange(IEnumerable<T> entities)
		{
			return _usersSet.RemoveRange(entities);
		}

		
	}
}