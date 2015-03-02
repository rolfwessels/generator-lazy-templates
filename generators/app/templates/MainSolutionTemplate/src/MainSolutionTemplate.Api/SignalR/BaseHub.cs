using System.Threading.Tasks;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.SignalR.Connection;
using MainSolutionTemplate.Core.Managers.Interfaces;
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

		public override Task OnConnected()
		{
			ConnectionState connectionState = _connectionsState.AddOrGet(Context);
			Groups.Add(Context.ConnectionId, connectionState.UserEmail);
			return base.OnConnected();
		}


		public override Task OnDisconnected(bool stopCalled)
		{
			ConnectionState connectionState = _connectionsState.Remove(Context.ConnectionId);
			Groups.Remove(Context.ConnectionId, connectionState.UserEmail);
			return base.OnDisconnected(stopCalled);
		}


		public override Task OnReconnected()
		{
			_connectionsState.Reconnect(Context);
			return base.OnReconnected();
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