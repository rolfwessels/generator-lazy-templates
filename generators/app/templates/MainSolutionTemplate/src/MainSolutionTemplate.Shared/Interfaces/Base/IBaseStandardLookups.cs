using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainSolutionTemplate.Shared.Interfaces.Base
{
    public interface IBaseStandardLookups<TDetails, TModelReference>
    {
        Task<IList<TModelReference>> Get();

        Task<IList<TDetails>> GetDetail();
    }
}