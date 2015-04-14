using System;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR.Connection;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Shared.Models;
using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Api.SignalR
{
	public abstract class BaseHub : Hub
	{
		protected readonly IConnectionStateMapping _connectionsState;
	    private readonly IMessenger _messenger;

	    protected BaseHub(IConnectionStateMapping connectionStateMapping)
		{
		    _connectionsState = connectionStateMapping;
		    _messenger = Messenger.Default;
		}

	    public override async Task OnConnected()
		{
			ConnectionState connectionState = _connectionsState.AddOrGet(Context);
			await Groups.Add(Context.ConnectionId, connectionState.UserEmail);
			await base.OnConnected();
		}

        public override async Task OnDisconnected(bool stopCalled)
		{
			ConnectionState connectionState = _connectionsState.Remove(Context.ConnectionId);
            await Groups.Remove(Context.ConnectionId, connectionState.UserEmail);
			await base.OnDisconnected(stopCalled);    
		}

		public override async Task OnReconnected()
		{
		    var connectionState = _connectionsState.Reconnect(Context);
		    await Groups.Add(Context.ConnectionId, connectionState.UserEmail);
			await base.OnReconnected();
		}


        protected void RegisterForDalUpdates<T, T2>(Func<ValueUpdateModel<T2>, Task> onUpdate)
        {
            var addOrGet = _connectionsState.AddOrGet(Context);
            _messenger.Register<DalUpdateMessage<T>>(addOrGet,
                                                                  r =>
                                                                  {
                                                                      onUpdate(r.ToValueUpdateModel<T, T2>())
                                                                          .Wait();
                                                                  });
        }


        protected void UnregisterFromDalUpdates<T>()
        {
            var addOrGet = _connectionsState.AddOrGet(Context);
            _messenger.Unregister<DalUpdateMessage<T>>(addOrGet);
        }

		
	}

	
}