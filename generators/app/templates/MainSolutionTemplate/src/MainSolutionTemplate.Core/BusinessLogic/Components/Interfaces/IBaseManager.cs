using System;
using System.Linq;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IBaseManager<T>
    {
        IQueryable<T> Get();
        T Get(Guid id);
        T Save(T project);
        T Delete(Guid id);
    }
}