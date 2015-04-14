using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.Tests.Fakes
{
    public class FakeGeneralUnitOfWork : IGeneralUnitOfWork
    {
        public FakeGeneralUnitOfWork()
        {
            Users = new FakeRepository<User>();
            Roles = new FakeRepository<Role>();
            Applications = new FakeRepository<Application>();
            Projects = new FakeRepository<Project>();
        }

        #region Implementation of IUnitOfWork

        public void Rollback()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            
        }

        #endregion

        #region Implementation of IGeneralUnitOfWork

        public IRepository<User> Users { get; private set; }
        public IRepository<Role> Roles { get; private set; }
        public IRepository<Application> Applications { get; private set; }
        public IRepository<Project> Projects { get; private set; }

        #endregion

        public class FakeRepository<T> : IRepository<T> where T : IBaseDalModel
        {
            private List<T> _list;
            private IQueryable<T> _asQueryable;

            public FakeRepository()
            {
                _list = new List<T>();
                ElementType = typeof (T);
                _asQueryable = _list.AsQueryable();
                Expression = _asQueryable.Expression;
                Provider = _asQueryable.Provider;
            }

            #region Implementation of IEnumerable

            public IEnumerator<T> GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            #region Implementation of IQueryable

            public Expression Expression { get; private set; }
            public Type ElementType { get; private set; }
            public IQueryProvider Provider { get; private set; }

            #endregion

            #region Implementation of IRepository<T>

            public T Add(T entity)
            {
                _list.Add(entity);
                entity.UpdateDate = DateTime.Now;
                return entity;
            }

            public IEnumerable<T> AddRange(IEnumerable<T> entities)
            {
                foreach (var entity in entities)
                {
                    Add(entity);
                }
                return entities;
            }

            public bool Remove(T entity)
            {
                var baseDalModelWithId = entity as IBaseDalModelWithId;
                if (baseDalModelWithId != null)
                {
                    var baseDalModelWithIds =
                        _list.Cast<IBaseDalModelWithId>().FirstOrDefault(x => x.Id == baseDalModelWithId.Id);
                    if (baseDalModelWithIds != null)
                    {
                        _list.Remove((T) baseDalModelWithIds);
                        return true;

                    }
                }
                return _list.Remove(entity);
            }

            public T Update(T entity)
            {
                Remove(entity);
                _list.Add(entity);
                entity.UpdateDate = DateTime.Now;
                return entity;
            }

            #endregion
        }
    }
}