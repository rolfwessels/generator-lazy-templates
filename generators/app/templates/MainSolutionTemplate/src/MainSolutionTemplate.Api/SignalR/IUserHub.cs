using System;
using System.Linq;
using MainSolutionTemplate.Api.Models;

namespace MainSolutionTemplate.Api.SignalR
{
	public interface IUserHub
	{
		UserModel Get(Guid id);
		UserModel Post(UserDetailModel user);
		UserModel Put(Guid id, UserDetailModel user);
		bool Delete(Guid id);
	}

}