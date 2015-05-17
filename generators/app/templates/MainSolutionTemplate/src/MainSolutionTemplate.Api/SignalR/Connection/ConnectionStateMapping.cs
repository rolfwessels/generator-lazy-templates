using System.Collections.Concurrent;
using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using log4net;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Dal.Models.Enums;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR.Connection
{
	public class ConnectionStateMapping : IConnectionStateMapping
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IUserManager _userManager;
		private ConcurrentDictionary<string, ConnectionState> _connections;
		
		public ConnectionStateMapping(IUserManager userManager)
		{
			_userManager = userManager;
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
					var userByEmail = _userManager.GetUserByEmail(principal.Identity.Name);
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

		public ConnectionState Reconnect(HubCallerContext context)
		{
			_log.Info("Reconnect.... " + _connections.ContainsKey(context.ConnectionId));
			return AddOrGet(context);
		}

		public bool IsAuthorized(ConnectionState connectionState, Activity[] activities)
		{
			return connectionState.IsAuthorized(activities);
		}
	}
}
