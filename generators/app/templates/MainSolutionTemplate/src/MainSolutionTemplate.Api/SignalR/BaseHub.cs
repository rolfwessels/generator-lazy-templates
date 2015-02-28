using System.Threading.Tasks;
using MainSolutionTemplate.Api.SignalR.Connnections;
using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Api.SignalR
{
    public abstract class BaseHub : Hub
    {
        private static bool _isFireOnceDone;
        private static readonly object _fireOnceLocker = new object();
        private readonly IConnectionStateMapping _connectionsState;


        protected BaseHub(IConnectionStateMapping connectionStateMapping)
        {
            _connectionsState = connectionStateMapping;
            FireInitializeOnce();
        }

        public override Task OnConnected()
        {
            ConnectionState connectionState = _connectionsState.AddOrGet(Context);
            SetDefaultGroups(connectionState);
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            _connectionsState.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        public override Task OnReconnected()
        {
            ConnectionState connectionState = _connectionsState.Reconnect(Context);
            SetDefaultGroups(connectionState);
            return base.OnReconnected();
        }


        protected virtual void OnInitializeOnce()
        {
        }

        #region Private Methods

        private void SetDefaultGroups(ConnectionState connectionState)
        {
            Groups.Add(Context.ConnectionId, connectionState.UserEmail);
        }

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