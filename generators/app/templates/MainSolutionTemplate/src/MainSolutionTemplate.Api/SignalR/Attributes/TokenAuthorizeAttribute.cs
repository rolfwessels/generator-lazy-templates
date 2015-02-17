using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models.Enums;
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

	public class HubAuthorizeActivityAttribute : AuthorizeAttribute
	{
		private IAuthorizeManager _authorizeManager;

		public HubAuthorizeActivityAttribute(params Activity[] activities)
		{
			_authorizeManager = IocContainerSetup.Instance.Resolve<IAuthorizeManager>();
			Activities = activities;
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
		{
			var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;
			var principal = environment["server.User"] as ClaimsPrincipal;

			if (principal != null && principal.Identity.IsAuthenticated)
			{

				return principal.Claims.Where(x => x.Type == ClaimTypes.Role).Any(x => _authorizeManager.IsAuthorizedActivity(Activities, x.Value));
			}

			return false;
		}

		public Activity[] Activities { get; private set; }
	}
}