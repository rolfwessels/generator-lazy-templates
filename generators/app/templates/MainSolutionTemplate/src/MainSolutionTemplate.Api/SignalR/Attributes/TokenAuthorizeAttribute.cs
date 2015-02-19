using System;
using System.Reflection;
using System.Security.Claims;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Security;
using log4net;

namespace MainSolutionTemplate.Api.SignalR.Attributes
{
	public class TokenAuthorizeAttribute : AuthorizeAttribute
	{
	
		public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
		{
			var authenticationTicket = GetPrincipal(request);
			if (authenticationTicket == null ||  !authenticationTicket.Identity.IsAuthenticated)
			{
				return false;
			}
			return true;
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
		{
			var principal = GetPrincipal(hubIncomingInvokerContext.Hub.Context.Request);
			if (principal != null && principal.Identity.IsAuthenticated)
			{
				return true;
			}
			return false;
		}

		private static ClaimsPrincipal GetPrincipal(IRequest request)
		{
			var principal = request.Environment["server.User"] as ClaimsPrincipal;
			if (principal != null && principal.Identity.IsAuthenticated) return principal;
			var token = request.QueryString.Get("Bearer");
			var authenticationTicket = OathAuthorizationSetup.OAuthOptions.AccessTokenFormat.Unprotect(token);
			if (authenticationTicket == null)  return null; 
			var claimsPrincipal = new ClaimsPrincipal(authenticationTicket.Identity);
			request.Environment["server.User"] = claimsPrincipal;
			return claimsPrincipal;
		}
	}
}