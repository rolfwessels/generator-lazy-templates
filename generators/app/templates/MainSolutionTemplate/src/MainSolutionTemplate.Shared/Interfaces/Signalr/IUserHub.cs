using System;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
	public interface IUserHub
	{
		UserModel Get(Guid id);
		UserModel Post(UserDetailModel user);
		UserModel Put(Guid id, UserDetailModel user);
		bool Delete(Guid id);
	}

}