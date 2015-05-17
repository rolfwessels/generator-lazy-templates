using System;
using System.Linq;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IBaseManager<T>
    {
        IQueryable<T> GetProjects();
        T GetProject(Guid id);
        T SaveProject(T project);
        T DeleteProject(Guid id);
    }
}