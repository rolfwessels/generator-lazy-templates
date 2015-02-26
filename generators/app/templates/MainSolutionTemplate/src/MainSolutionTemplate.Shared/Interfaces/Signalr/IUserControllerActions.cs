using System;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
	public interface IUserControllerActions
	{
		UserModel Get(Guid id);
		UserModel Post(UserDetailModel user);
		UserModel Put(Guid id, UserDetailModel user);
		bool Delete(Guid id);

	    UserModel Register(RegisterModel user);
	    bool ForgotPassword(string email);
	}

}