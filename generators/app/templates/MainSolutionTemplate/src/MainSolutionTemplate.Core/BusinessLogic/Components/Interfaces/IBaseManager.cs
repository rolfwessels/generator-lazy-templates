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
        T Get(Guid id);
        T Save(T project);
        T Delete(Guid id);
    }
}