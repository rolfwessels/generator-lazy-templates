using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
	public interface IAuthorizeManager
	{
		bool IsAuthorizedActivity(Activity[] activities, string roleName);
	}
}