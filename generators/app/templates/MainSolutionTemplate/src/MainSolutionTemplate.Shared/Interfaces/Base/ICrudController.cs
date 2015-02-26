using System;
using System.Threading.Tasks;

namespace MainSolutionTemplate.Shared.Interfaces.Base
{
    public interface ICrudController<TModel, TDetailModel>
    {
        Task<TModel> Get(Guid id);
        Task<TModel> Post(TDetailModel user);
        Task<TModel> Put(Guid id, TDetailModel user);
        Task<bool> Delete(Guid id);
    }
}