using System;
using System.Linq;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.Managers.Interfaces
{
	public interface IUserManager
	{
		IQueryable<User> GetUsers();
		User GetUser(Guid id);
		User SaveUser(User user);
		User DeleteUser(Guid id);
		User GetUserByEmailAndPassword(string email, string password);
		User GetUserByEmail(string email);
	}
}