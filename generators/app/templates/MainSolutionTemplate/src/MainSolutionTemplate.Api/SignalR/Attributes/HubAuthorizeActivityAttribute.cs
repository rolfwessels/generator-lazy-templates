using System.Linq;
using System.Security.Claims;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.SignalR.Connnections;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models.Enums;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR.Attributes
{
	public class HubAuthorizeActivityAttribute : AuthorizeAttribute
	{
		private static readonly IConnectionStateMapping _connectionStateMapping;
		

		static HubAuthorizeActivityAttribute()
		{
					
				_connectionStateMapping = IocApi.Instance.Resolve<IConnectionStateMapping>();
		}

		public HubAuthorizeActivityAttribute(params Activity[] activities)
		{
			Activities = activities;
		}

		public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
		{
			var connectionState = _connectionStateMapping.AddOrGet(hubIncomingInvokerContext.Hub.Context);
			return _connectionStateMapping.IsAuthorized(connectionState, Activities);
		}

		public Activity[] Activities { get; private set; }
	}

	
}
