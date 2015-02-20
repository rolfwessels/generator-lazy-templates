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
			var authenticationTicket = request.GetPrincipal();
			if (authenticationTicket == null ||  !authenticationTicket.Identity.IsAuthenticated)
			{
				return false;
			}
			return true;
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
		{
			var principal = hubIncomingInvokerContext.Hub.Context.Request.GetPrincipal();
			if (principal != null && principal.Identity.IsAuthenticated)
			{
				return true;
			}
			return false;
		}
	}

	public static class RequestUserDetailsHelper {
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public static ClaimsPrincipal GetPrincipal(this IRequest request)
		{
			var principal = request.Environment["server.User"] as ClaimsPrincipal;
			if (principal != null && principal.Identity.IsAuthenticated) return principal;
			_log.Info(string.Format("request.QueryString: [{0}]", request.QueryString.Get(Constants.TokenQueryStringParam)));
			var token = request.QueryString.Get(Constants.TokenQueryStringParam);
			var authenticationTicket = OathAuthorizationSetup.OAuthOptions.AccessTokenFormat.Unprotect(token);
			if (authenticationTicket == null)  return null; 
			var claimsPrincipal = new ClaimsPrincipal(authenticationTicket.Identity);
			request.Environment["server.User"] = claimsPrincipal;
			request.Environment["server.UserName"] = claimsPrincipal.Identity.Name;
			return claimsPrincipal;
		}

		public static string GetUserName(this IRequest request)
		{
			var userName = request.Environment["server.UserName"] as string;
			if (!string.IsNullOrEmpty(userName)) return userName;
			var claimsPrincipal = request.GetPrincipal();
			request.Environment["server.UserName"] = claimsPrincipal.Identity.Name;
			return claimsPrincipal.Identity.Name;
		}
	}
}