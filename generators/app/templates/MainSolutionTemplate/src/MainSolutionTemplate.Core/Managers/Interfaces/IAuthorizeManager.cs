using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Core.Managers.Interfaces
{
	public interface IAuthorizeManager
	{
		bool IsAuthorizedActivity(Activity[] activities, string roleName);
	}
}