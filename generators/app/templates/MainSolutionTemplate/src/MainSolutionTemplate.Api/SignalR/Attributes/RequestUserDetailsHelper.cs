using System;
using System.Reflection;
using System.Security.Claims;
using Lazy.Authentication.OAuth2;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security;
using log4net;

namespace MainSolutionTemplate.Api.SignalR.Attributes
{
    public static class RequestUserDetailsHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ClaimsPrincipal GetPrincipal(this IRequest request)
        {
            try
            {
                _log.Info(string.Format("request.QueryString: [{0}]",
                                        request.QueryString.Get(Constants.TokenQueryStringParam)));
                string token = request.QueryString.Get(Constants.TokenQueryStringParam);
                AuthenticationTicket authenticationTicket =
                    OathAuthorizationSetup.OAuthOptions.AccessTokenFormat.Unprotect(token);
                if (authenticationTicket == null) return null;
                var claimsPrincipal = new ClaimsPrincipal(authenticationTicket.Identity);
                return claimsPrincipal;
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
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