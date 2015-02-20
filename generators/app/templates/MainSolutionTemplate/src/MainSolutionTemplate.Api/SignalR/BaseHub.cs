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
using MainSolutionTemplate.Api.SignalR.Attributes;

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
			

			// Presence Related            
			_connections.Add(Context.Request.GetUserName(), connectionId);
			_log.Info(string.Format("Welcome userId {0}", Context.Request.GetUserName()));

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
			// Presence Related            
			_connections.Remove(Context.Request.GetUserName(), Context.ConnectionId);
			_log.Info(string.Format("UserName {0} has left ", Context.Request.GetUserName()));
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
			
			if (!_connections.GetConnections(Context.Request.GetUserName()).Contains(connectionId))
            {
				_connections.Add(Context.Request.GetUserName(), Context.ConnectionId);
            }
			return base.OnReconnected();
		}

		

		
	}
}