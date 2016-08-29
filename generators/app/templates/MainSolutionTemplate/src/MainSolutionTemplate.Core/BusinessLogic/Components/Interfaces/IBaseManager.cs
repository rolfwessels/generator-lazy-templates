using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IBaseManager<T>
    {
        Task<List<T>> Get();
        Task<List<T>> Get(Expression<Func<T, bool>> filter);
        Task<T> GetById(string id);
        Task<T> Delete(string id);
        IQueryable<T> Query();
        Task<T> Update(T entity);
        Task<T> Insert(T entity);
    }
}