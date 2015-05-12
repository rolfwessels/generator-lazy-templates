using System;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Base;
using Microsoft.AspNet.SignalR.Client;

namespace MainSolutionTemplate.Sdk.SignalrClient.Base
{
    public abstract class BaseCrudHubClient<TModel, TReferenceModel, TDetailModel> :
        BaseGetHubClient<TModel, TReferenceModel>, ICrudController<TModel, TDetailModel>
    {
        protected BaseCrudHubClient(HubConnection hubConnection) : base(hubConnection)
        {
        }

        #region ICrudController<TModel,TDetailModel> Members

        public Task<TModel> Get(Guid id)
        {
            return _userHub.Invoke<TModel>("Get", id);
        }

        public Task<TModel> Post(TDetailModel model)
        {
            return _userHub.Invoke<TModel>("Post", model);
        }

        public Task<TModel> Put(Guid id, TDetailModel model)
        {
            return _userHub.Invoke<TModel>("Put", id, model);
        }

        public Task<bool> Delete(Guid id)
        {
            return _userHub.Invoke<bool>("Delete", id);
        }

        #endregion
    }
}