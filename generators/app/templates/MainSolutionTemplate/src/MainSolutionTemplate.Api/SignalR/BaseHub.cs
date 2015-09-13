using System.Threading.Tasks;
using MainSolutionTemplate.Api.SignalR.Connection;
using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Api.SignalR
{
	public abstract class BaseHub : Hub
	{
		protected readonly IConnectionStateMapping _connectionsState;

	    protected BaseHub(IConnectionStateMapping connectionStateMapping)
		{
		    _connectionsState = connectionStateMapping;
		    
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


      
		
	}

	
}