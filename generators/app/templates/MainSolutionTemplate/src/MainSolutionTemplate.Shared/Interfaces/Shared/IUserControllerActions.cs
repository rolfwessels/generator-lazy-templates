using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
    public interface IUserControllerActions : ICrudController<UserModel, UserDetailModel> 
	{
	    Task<UserModel> Register(RegisterModel user);
	    Task<bool> ForgotPassword(string email);
	}
}