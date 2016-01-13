using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Core.Tests.Fakes
{
    public class FakeGeneralUnitOfWork : IGeneralUnitOfWork
    {
        public FakeGeneralUnitOfWork()
        {
            Users = new FakeRepository<User>();
            Applications = new FakeRepository<Application>();
            Projects = new FakeRepository<Project>();
        }


        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region Implementation of IGeneralUnitOfWork

        public IRepository<User> Users { get; private set; }
        public IRepository<Application> Applications { get; private set; }
        public IRepository<Project> Projects { get; private set; }

        #endregion

        #region Nested type: FakeRepository

        public class FakeRepository<T> : IRepository<T> where T : IBaseDalModel
        {
            private readonly List<T> _list;

            public FakeRepository()
            {
                _list = new List<T>();
            }

            #region Implementation of IRepository<T>

            public IQueryable<T> Query()
            {
                return _list.AsQueryable();
            }

            public Task<T> Add(T entity)
            {
                entity.CreateDate = DateTime.Now;
                AddAndSetUpdateDate(entity);
                return Task.FromResult(entity);
            }


            public IEnumerable<T> AddRange(IEnumerable<T> entities)
            {
                var addRange = entities as T[] ?? entities.ToArray();
                foreach (T entity in addRange)
                {
                    Add(entity);
                }
                return addRange;
            }

            public Task<bool> Remove(Expression<Func<T, bool>> filter)
            {
                T[] array = _list.Where(filter.Compile()).ToArray();
                array.ForEach(x => _list.Remove(x));
                return Task.FromResult(array.Length > 0);
            }

            public Task<List<T>> Find(Expression<Func<T, bool>> filter)
            {
                return Task.FromResult(_list.Where(filter.Compile()).ToList());
            }

            public Task<T> FindOne(Expression<Func<T, bool>> filter)
            {
                return Task.FromResult(_list.FirstOrDefault(filter.Compile()));
            }

            public Task<long> Count()
            {
                return Task.FromResult(_list.LongCount());
            }

            public Task<long> Count(Expression<Func<T, bool>> filter)
            {
                return Task.FromResult(_list.Where(filter.Compile()).LongCount());
            }

            public Task<T> Update(Expression<Func<T, bool>> filter, T entity)
            {
                Remove(filter);
                AddAndSetUpdateDate(entity);
                return Task.FromResult(entity);
            }

            public bool Remove(T entity)
            {
                var baseDalModelWithId = entity as IBaseDalModelWithId;
                if (baseDalModelWithId != null)
                {
                    IBaseDalModelWithId baseDalModelWithIds =
                        _list.Cast<IBaseDalModelWithId>().FirstOrDefault(x => x.Id == baseDalModelWithId.Id);
                    if (baseDalModelWithIds != null)
                    {
                        _list.Remove((T) baseDalModelWithIds);
                        return true;
                    }
                }
                return _list.Remove(entity);
            }

            public T Update(T entity, object t)
            {
                Remove(entity);
                AddAndSetUpdateDate(entity);
                return entity;
            }

            #endregion

            #region Private Methods

            private void AddAndSetUpdateDate(T entity)
            {
                _list.Add(entity);
                entity.UpdateDate = DateTime.Now;
            }

            #endregion
        }

        #endregion
    }
}