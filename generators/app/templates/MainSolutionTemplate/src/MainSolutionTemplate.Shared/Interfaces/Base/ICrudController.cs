using System;
using System.Threading.Tasks;

namespace MainSolutionTemplate.Shared.Interfaces.Base
{
    public interface ICrudController<TModel, TDetailModel>
    {
        Task<TModel> Get(Guid id);
        Task<TModel> Insert(TDetailModel model);
        Task<TModel> Update(Guid id, TDetailModel model);
        Task<bool> Delete(Guid id);
    }
}