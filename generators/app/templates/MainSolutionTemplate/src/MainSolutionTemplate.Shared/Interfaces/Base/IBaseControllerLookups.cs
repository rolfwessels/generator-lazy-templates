using System.Linq;
using System.Threading.Tasks;

namespace MainSolutionTemplate.Shared.Interfaces.Base
{
    public interface IBaseControllerLookups<TDetails, TModelReference>
    {
        Task<IQueryable<TModelReference>> Get();

        Task<IQueryable<TDetails>> GetDetail();
    }
}