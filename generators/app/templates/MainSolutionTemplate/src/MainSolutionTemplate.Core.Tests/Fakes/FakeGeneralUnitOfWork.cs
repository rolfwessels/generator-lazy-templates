using System;
using System.Collections;
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

            public FakeRepository()
            {
                _list = new List<T>();
            }

           

            #region Implementation of IRepository<T>

            public T Add(T entity)
            {
                entity.CreateDate = DateTime.Now;
                AddAndSetUpdateDate(entity);
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

            public bool Remove(Expression<Func<T, bool>> filter)
            {
                T[] array = _list.Where(filter.Compile()).ToArray();
                array.ForEach(x => _list.Remove(x));
                return array.Length > 0;
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

            public T Update(Expression<Func<T, bool>> filter, T entity)
            {
                Remove(filter);
                AddAndSetUpdateDate(entity);
                return entity;

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

            public T Update(T entity, object t)
            {
                Remove(entity);
                AddAndSetUpdateDate(entity);
                return entity;
            }

            #endregion

            private void AddAndSetUpdateDate(T entity)
            {
                _list.Add(entity);
                entity.UpdateDate = DateTime.Now;
            }

        }
    }
}