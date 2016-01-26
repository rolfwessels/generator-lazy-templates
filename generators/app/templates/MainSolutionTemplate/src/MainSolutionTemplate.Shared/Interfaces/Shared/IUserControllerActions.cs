using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
    public interface IUserControllerActions : ICrudController<UserModel, UserCreateUpdateModel>
	{
	    Task<UserModel> Register(RegisterModel user);
	    Task<bool> ForgotPassword(string email);
        Task<UserModel> WhoAmI();
        Task<List<RoleModel>> Roles();
	}
}