using System.Linq;
using System.Security.Claims;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models.Enums;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR.Attributes
{
	public class HubAuthorizeActivityAttribute : AuthorizeAttribute
	{
		private readonly IAuthorizeManager _authorizeManager;

		public HubAuthorizeActivityAttribute(params Activity[] activities)
		{
			_authorizeManager = IocApi.Instance.Resolve<IAuthorizeManager>();
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