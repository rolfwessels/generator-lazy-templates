using System.Collections.Concurrent;
using System.Reflection;
using log4net;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models.Enums;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR.Connection
{
	public class ConnectionStateMapping : IConnectionStateMapping
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly ISystemManager _systemManager;
		private ConcurrentDictionary<string, ConnectionState> _connections;
		private IAuthorizeManager _authorizeManager;
		public ConnectionStateMapping(ISystemManager systemManager, IAuthorizeManager authorizeManager)
		{
			_systemManager = systemManager;
			_authorizeManager = authorizeManager;
			_connections = new ConcurrentDictionary<string, ConnectionState>();
		}

		public int Count
		{
			get { return _connections.Count; }
		}

	

		public ConnectionState AddOrGet(HubCallerContext context )
		{
			return _connections.GetOrAdd(context.ConnectionId, (t) =>
				{
					var principal = context.Request.GetPrincipal();
					var userByEmail = _systemManager.GetUserByEmail(principal.Identity.Name);
					var connectionState = new ConnectionState(context.ConnectionId, principal, userByEmail);
					return connectionState;
				});
		}

		public ConnectionState Remove(string connectionId)
		{
			ConnectionState removed;
			_connections.TryRemove(connectionId, out removed);
			return removed;
		}

		public void Reconnect(HubCallerContext context)
		{
			_log.Info("Reconnect.... " + _connections.ContainsKey(context.ConnectionId));
			AddOrGet(context);
		}

		public bool IsAuthorized(ConnectionState connectionState, Activity[] activities)
		{
			return connectionState.IsAuthorized(activities);
		}
	}
}
