using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using log4net;

namespace MainSolutionTemplate.Api.SignalR.Attributes
{
	public static class RequestUserDetailsHelper {
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public static ClaimsPrincipal GetPrincipal(this IRequest request)
		{
			try
			{
				var token = request.QueryString.Get(Constants.TokenQueryStringParam);
				var authenticationTicket = OathAuthorizationSetup.OAuthOptions.AccessTokenFormat.Unprotect(token);
				if (authenticationTicket == null)  return null; 
				var claimsPrincipal = new ClaimsPrincipal(authenticationTicket.Identity);
				return claimsPrincipal;
			}
			catch (Exception e)
			{
				_log.Error(e.Message, e);
				throw;
			}
		}

		public static string GetUserName(this IRequest request)
		{
			var claimsPrincipal = request.GetPrincipal();
			if (claimsPrincipal != null) return claimsPrincipal.Identity.Name;
			return null;
		}

		public static bool IsAuthenticated(this ClaimsPrincipal principal)
		{
			if (principal != null && principal.Identity.IsAuthenticated)
			{
				return true;
			}
			return false;
		}
		
		
	}
}