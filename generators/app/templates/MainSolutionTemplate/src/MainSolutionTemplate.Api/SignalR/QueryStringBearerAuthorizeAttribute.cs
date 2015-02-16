using System;
using System.Reflection;
using System.Security.Claims;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using log4net;

namespace MainSolutionTemplate.Api.SignalR
{
	public class QueryStringBearerAuthorizeAttribute : AuthorizeAttribute
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


		public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
		{
			var token = request.QueryString.Get("Bearer");
			var authenticationTicket = OathAuthorizationSetup.OAuthOptions.AccessTokenFormat.Unprotect(token);

			if (authenticationTicket == null || authenticationTicket.Identity == null || !authenticationTicket.Identity.IsAuthenticated)
			{
				return false;
			}

			request.Environment["server.User"] = new ClaimsPrincipal(authenticationTicket.Identity);
			request.Environment["server.Username"] = authenticationTicket.Identity.Name;
			//UserAuthorized(new ClaimsPrincipal(authenticationTicket.Identity));
			return true;
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
		{
			

			// check the authenticated user principal from environment
			var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;
			var principal = environment["server.User"] as ClaimsPrincipal;

			if (principal != null && principal.Identity.IsAuthenticated)
			{
				// create a new HubCallerContext instance with the principal generated from token
				// and replace the current context so that in hubs we can retrieve current user identity
				//var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;
				//hubIncomingInvokerContext.Hub.Context = new HubCallerContext(new Microsoft.AspNet.SignalR.Owin.ServerRequest(environment), connectionId);

				return true;
			}

			return false;
		}

	}
}