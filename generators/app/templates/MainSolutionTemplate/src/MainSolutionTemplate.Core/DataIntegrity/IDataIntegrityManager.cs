using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.DataIntegrity
{
    public interface IDataIntegrityManager
    {
        Task<long> UpdateAllReferences<T>(T updatedValue);
        Task<long> GetReferenceCount<T>(T updatedValue);
    }
}