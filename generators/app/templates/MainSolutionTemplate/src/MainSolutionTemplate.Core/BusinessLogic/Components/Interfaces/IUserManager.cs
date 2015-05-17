using System;
using System.Linq;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
	public interface IUserManager
	{
		IQueryable<User> GetUsers();
        IQueryable<UserReference> GetUsersAsReference();
		User GetUser(Guid id);
        User SaveUser(User user, string password = null);
		User DeleteUser(Guid id);
		User GetUserByEmailAndPassword(string email, string password);
		User GetUserByEmail(string email);
	    void UpdateLastLoginDate(string email);
	}

    
}