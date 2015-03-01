using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Shared.Interfaces.Base
{
    public interface IBaseStandardLookups<TDetails, TModelReference>
    {
        Task<List<UserReferenceModel>> Get();

        Task<List<UserModel>> GetDetail();
    }
}