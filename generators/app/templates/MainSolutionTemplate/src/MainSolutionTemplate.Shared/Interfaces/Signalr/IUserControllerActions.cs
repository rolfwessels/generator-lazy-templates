using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
    public interface IUserControllerActions : ICrud<UserModel, UserDetailModel>
	{
	    Task<UserModel> Register(RegisterModel user);
	    Task<bool> ForgotPassword(string email);
	}
}