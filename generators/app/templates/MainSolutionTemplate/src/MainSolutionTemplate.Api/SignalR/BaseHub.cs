using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security;
using log4net;

namespace MainSolutionTemplate.Api.SignalR
{
	public class BaseHub : Hub
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly ConnectionMapping<string> _connections = new ConnectionMapping<string>();
		private ISystemManagerFacade _systemManagerFacade;
		private ISecureDataFormat<AuthenticationTicket> _accessTokenFormat;

		public BaseHub()
		{
			_systemManagerFacade = IocContainerSetup.Instance.Resolve<ISystemManagerFacade>();
			_accessTokenFormat = OathAuthorizationSetup.OAuthOptions.AccessTokenFormat;
		}


		/// <summary>
		/// The on connected.
		/// </summary>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public override Task OnConnected()
		{
			// Identity Related
			var connectionId = Context.ConnectionId;
			var userName = this.GetCurrentUserName();
			var userId = this.GetCurrentUserId(userName);

			// Presence Related            
			_connections.Add(userName, connectionId);
			Groups.Add(connectionId, "Room1");
			_log.Info(string.Format("Welcome userId {0} {1}", userName, userId));

			return base.OnConnected();
		}

		/// <summary>
		/// The on disconnected.
		/// </summary>
		/// <param name="stopCalled">
		/// The stop called.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public override Task OnDisconnected(bool stopCalled)
		{
			// Identity Related
			var connectionId = Context.ConnectionId;
			var userName = this.GetCurrentUserName();

			// Presence Related            
			_connections.Remove(userName, Context.ConnectionId);
			Groups.Remove(connectionId, "Room1");
			_log.Info(string.Format("userName {0} has left ", userName));
			return base.OnDisconnected(stopCalled);
		}

		/// <summary>
		/// The on reconnected.
		/// </summary>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public override Task OnReconnected()
		{
			var connectionId = Context.ConnectionId;
			var userName = this.GetCurrentUserName();
			var userId = this.GetCurrentUserId(userName);

			Groups.Add(connectionId, "Room1");
			_log.Info(string.Format("userName {0} {1} has come back ", userName , userId));
			if (!_connections.GetConnections(userName).Contains(connectionId))
            {
                _connections.Add(userName, Context.ConnectionId);
            }
			return base.OnReconnected();
		}

		

		private string GetCurrentUserName()
		{
			var principle = Context.Request.Environment["server.User"] as ClaimsPrincipal;
			if (principle != null)
			{
				return principle.Identity.Name;
			}
			principle = Context.User as ClaimsPrincipal;
			if (principle != null)
			{
				return principle.Identity.Name;
			}
			var token = Context.Request.QueryString.Get("Bearer");
			var authenticationTicket = _accessTokenFormat.Unprotect(token);
			return authenticationTicket.Identity.Name;
		}

		private Guid? GetCurrentUserId(string userName)
		{
			var userByEmail = _systemManagerFacade.GetUserByEmail(userName);
			if (userByEmail != null) return userByEmail.Id;
			return null;
		}
	}
}