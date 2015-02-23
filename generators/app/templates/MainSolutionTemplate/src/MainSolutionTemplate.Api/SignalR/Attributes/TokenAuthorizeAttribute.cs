using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Security;

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
}