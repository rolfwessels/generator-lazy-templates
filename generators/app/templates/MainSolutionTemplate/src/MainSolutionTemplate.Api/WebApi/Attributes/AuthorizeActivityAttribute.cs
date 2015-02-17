using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models.Enums;
using log4net;

namespace MainSolutionTemplate.Api.WebApi.Attributes
{
	public class AuthorizeActivityAttribute : AuthorizeAttribute
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly Activity[] _activities;



		public AuthorizeActivityAttribute(params Activity[] activities)
		{
			_activities = activities;
		}

		public Activity[] Activities
		{
			get { return _activities; }
		}

	
		#region Overrides of AuthorizeAttribute

		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var isAuthorized = base.IsAuthorized(actionContext);
			if (isAuthorized)
			{
				var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
				if (identity == null)
				{
					_log.Error("User not authorized because we were expecting a ClaimsIdentity");
					return false;
				}
				var authorizeManager = IocContainerSetup.Instance.Resolve<IAuthorizeManager>();
				isAuthorized = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Any(x => authorizeManager.IsAuthorizedActivity(Activities, x.Value));
			}
			return isAuthorized;
		}

		#endregion



	}
}