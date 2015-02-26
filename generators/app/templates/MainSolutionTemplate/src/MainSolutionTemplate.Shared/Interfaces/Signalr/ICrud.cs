using System;
using System.Threading.Tasks;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
    public interface ICrud<TModel, TDetailModel>
    {
        Task<TModel> Get(Guid id);
        Task<TModel> Post(TDetailModel user);
        Task<TModel> Put(Guid id, TDetailModel user);
        Task<bool> Delete(Guid id);
    }
}