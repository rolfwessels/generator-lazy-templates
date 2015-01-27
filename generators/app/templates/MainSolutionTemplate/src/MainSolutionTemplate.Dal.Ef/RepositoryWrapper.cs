using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Dal.Ef
{
	public class RepositoryWrapper<T> : IRepository<T> where T : class
	{
		private readonly DbSet<T> _usersSet;
		private readonly GeneralDbContext _value;
		private IQueryable<T> _asQueryable;

		public RepositoryWrapper(DbSet<T> usersSet, GeneralDbContext value)
		{
			_usersSet = usersSet;
			_value = value;
			_asQueryable = _usersSet.AsNoTracking();
		}

		
		public T Get(object id)
		{
			throw new NotImplementedException();
		}

		public T Add(T entity)
		{
			T add = _usersSet.Add(entity);
			_value.SaveChanges();
			return add;
		}

		public IEnumerable<T> AddRange(IEnumerable<T> entities)
		{
			IEnumerable<T> addRange = _usersSet.AddRange(entities);
			_value.SaveChanges();
			return addRange;
		}

		public bool Remove(T entity)
		{
			_usersSet.Remove(entity);
			return _value.SaveChanges() > 0;
		}

		public int RemoveRange(IEnumerable<T> entities)
		{
			var enumerable = entities as T[] ?? entities.ToArray();
			_usersSet.RemoveRange(enumerable);
			return _value.SaveChanges();
			
		}

		#region Implementation of IEnumerable

		public IEnumerator<T> GetEnumerator()
		{
			return _asQueryable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of IQueryable

		public Expression Expression {
			get { return _asQueryable.Expression; }
		}

		public Type ElementType
		{
			get { return _asQueryable.ElementType; }
		}

		public IQueryProvider Provider
		{
			get { return _asQueryable.Provider; }
		}

		#endregion
	}
}