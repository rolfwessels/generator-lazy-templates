using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Shared.Interfaces.Base
{
    public interface IBaseControllerLookups<TDetails, TModelReference>
    {
        Task<IEnumerable<UserReferenceModel>> Get();

        Task<IEnumerable<UserModel>> GetDetail();
    }
}