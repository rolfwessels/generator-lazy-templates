using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
    public interface IUserControllerLookups
    {
        Task<IQueryable<UserReferenceModel>> Get();

        Task<IQueryable<UserModel>> GetDetail();
    }

    public interface IUserControllerStandardLookups
    {

        Task<List<UserReferenceModel>> Get();

        Task<List<UserModel>> GetDetail();
    }
}