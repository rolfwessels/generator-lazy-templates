using System;
using System.Collections.Concurrent;
using System.Reflection;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Core.Managers.Interfaces;
using Microsoft.AspNet.SignalR.Hubs;
using log4net;

namespace MainSolutionTemplate.Api.SignalR
{
	public class ConnectionStateMapping : IConnectionStateMapping
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly ISystemManager _systemManager;
		private ConcurrentDictionary<string, ConnectionState> _connections;

		public ConnectionStateMapping(ISystemManager systemManager)
		{
			_systemManager = systemManager;
			_connections = new ConcurrentDictionary<string, ConnectionState>();
		}

		public int Count
		{
			get { return _connections.Count; }
		}

		public void Add(HubCallerContext context)
		{
			Add(context, null);
		}

		public ConnectionState Add(HubCallerContext context , Action<ConnectionState> connectionBuild)
		{
			return _connections.GetOrAdd(context.ConnectionId, (t) =>
				{
					var principal = context.Request.GetPrincipal();
					var userByEmail = _systemManager.GetUserByEmail(principal.Identity.Name);
					var connectionState = new ConnectionState(context.ConnectionId, principal, userByEmail);
					if (connectionBuild != null) connectionBuild(connectionState);
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
			Add(context);
		}
	}
}
