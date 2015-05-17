using System;
using System.Linq;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IApplicationManager
    {
        IQueryable<Application> GetApplications();
        Application GetApplication(Guid id);
        Application GetApplicationById(string clientId);
        Application SaveApplication(Application project);
        Application DeleteApplication(Guid id);
    }
}