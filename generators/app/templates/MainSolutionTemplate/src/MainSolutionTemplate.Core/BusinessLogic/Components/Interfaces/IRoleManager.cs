using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IRoleManager 
    {
        Task<Role> GetRoleByName(string name);
        Task<List<Role>> Get();
    }
}