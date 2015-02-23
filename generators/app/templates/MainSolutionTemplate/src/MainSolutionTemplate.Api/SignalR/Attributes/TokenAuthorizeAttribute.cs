using System.Security.Claims;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR.Attributes
{
	public class TokenAuthorizeAttribute : AuthorizeAttribute
	{
		public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
		{
			ClaimsPrincipal principal = request.GetPrincipal();
			return principal.IsAuthenticated();
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext,
		                                                  bool appliesToMethod)
		{
			ClaimsPrincipal principal = hubIncomingInvokerContext.Hub.Context.Request.GetPrincipal();
			return principal.IsAuthenticated();
		}

		
	}
}