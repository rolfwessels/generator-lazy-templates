using System.Threading.Tasks;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers.Interfaces;
using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Api.SignalR
{
	public abstract class BaseHub : Hub
	{
		private readonly IConnectionStateMapping _connectionsState;
		private static bool _isFireOnceDone;
		private static readonly object _fireOnceLocker = new object();



		public BaseHub(IConnectionStateMapping connectionStateMapping)
		{
			_connectionsState = connectionStateMapping;
			FireInitializeOnce();

		}


		/// <summary>
		///    The on connected.
		/// </summary>
		/// <returns>
		///    The <see cref="Task" /> .
		/// </returns>
		public override Task OnConnected()
		{
			ConnectionState connectionState = _connectionsState.Add(Context, OnInitialConnection);
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


		protected virtual void OnInitialConnection(ConnectionState connectionState)
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