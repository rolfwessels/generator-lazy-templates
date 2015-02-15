using System;
using System.Linq;
using MainSolutionTemplate.Api.Models;

namespace MainSolutionTemplate.Api.SignalR
{
	public interface IUserHub
	{
		
		UserModel Get(Guid id);
		UserModel Post(UserModel user);
		UserModel Put(Guid id, UserModel user);
		bool Delete(Guid id);
	}
}