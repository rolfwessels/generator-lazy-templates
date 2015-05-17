using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IApplicationManager : IBaseManager<Application>
    {
        Application GetApplicationById(string clientId);
    }
}