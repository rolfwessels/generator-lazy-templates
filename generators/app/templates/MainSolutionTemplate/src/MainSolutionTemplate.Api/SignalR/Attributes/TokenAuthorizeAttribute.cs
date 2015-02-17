using System;
using System.Reflection;
using System.Security.Claims;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using log4net;

namespace MainSolutionTemplate.Api.SignalR.Attributes
{
	public class TokenAuthorizeAttribute : AuthorizeAttribute
	{
	
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

			return true;
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
		{
			var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;
			var principal = environment["server.User"] as ClaimsPrincipal;

			if (principal != null && principal.Identity.IsAuthenticated)
			{
				return true;
			}

			return false;
		}
		

	}
}