using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.Core.BusinessLogic.Facade.Interfaces
{
	public interface ISystemManager : IOAuthDataManager, IUserManager , IRoleManager , IProjectManager
	{
		
	}
}