using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.BusinessLogic.Facade.Interfaces;
using MainSolutionTemplate.Dal.Models.Enums;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
	public class AuthorizeManager : IAuthorizeManager
	{
		public const string AdministratorRoleName = "Admin";

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly ISystemManager _systemManager;

		public AuthorizeManager(ISystemManager systemManager)
		{
			_systemManager = systemManager;
		}

		#region Implementation of IAuthorizeManager

		public bool IsAuthorizedActivity(Activity[] activities, string roleName)
		{
			if (roleName == AdministratorRoleName) return true;
			var rolesByName = _systemManager.GetRoleByName(roleName);
			if (rolesByName == null)
			{
				_log.Warn("AuthorizeManager:IsAuthorizedActivity Claim has a role called {0}. Currently we do not support that role");
				return false;
			}
			return activities.All(activity => rolesByName.Activities.Contains(activity));
		}

		#endregion
	}
}