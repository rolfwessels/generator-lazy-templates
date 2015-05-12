using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using Microsoft.AspNet.SignalR.Client;

namespace MainSolutionTemplate.Sdk.SignalrClient.Base
{
    public abstract class BaseGetHubClient<TModel, TReferenceModel> : BaseHubClient,
                                                                      IBaseStandardLookups<TModel, TReferenceModel>,
                                                                      IEventUpdateEventSubSubscription<TModel>
    {
        protected BaseGetHubClient(HubConnection hubConnection) : base(hubConnection)
        {
        }

        #region IBaseStandardLookups<TModel,TReferenceModel> Members

        public Task<IList<TReferenceModel>> Get()
        {
            return _userHub.Invoke<IList<TReferenceModel>>("Get");
        }

        public Task<IList<TModel>> GetDetail()
        {
            return _userHub.Invoke<IList<TModel>>("GetDetail");
        }

        #endregion

        #region IEventUpdateEventSubSubscription<TModel> Members

        public Task UnsubscribeFromUpdates()
        {
            return Task.Run(() => _userHub.Invoke("UnsubscribeFromUpdates"));
        }

        public Task OnUpdate(Action<ValueUpdateModel<TModel>> callBack)
        {
            return Task.Run(() =>
                {
                    _userHub.On("OnUpdate", callBack);
                    _userHub.Invoke("SubscribeToUpdates");
                });
        }

        #endregion
    }
}