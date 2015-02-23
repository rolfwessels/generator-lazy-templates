using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.Core.Managers.Interfaces
{
	public interface ISystemManager : IOAuthDataManager, IUserManager , IRoleManager
	{
		
	}
}