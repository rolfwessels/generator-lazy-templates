using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IRoleManager 
    {
        Role GetRoleByName(string name);
    }
}