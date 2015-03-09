using System.Threading.Tasks;
using MainSolutionTemplate.Api.SignalR.Connection;
using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Api.SignalR
{
	public abstract class BaseHub : Hub
	{
		private readonly IConnectionStateMapping _connectionsState;
		private static bool _isFireOnceDone;
		private static readonly object _fireOnceLocker = new object();


		protected BaseHub(IConnectionStateMapping connectionStateMapping)
		{
			_connectionsState = connectionStateMapping;
			FireInitializeOnce();
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

		protected virtual void OnInitializeOnce()
		{
		}

		#region Private Methods

		private void FireInitializeOnce()
		{
			if (_isFireOnceDone) return;
			lock (_fireOnceLocker)
			{
				if (!_isFireOnceDone)
				{
					OnInitializeOnce();
					_isFireOnceDone = true;
				}
			}
		}

		#endregion
	}

	
}