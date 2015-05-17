using System;
using System.Linq;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
	public interface IUserManager : IBaseManager<User>
	{
	    User Save(User user, string password = null);
	    User GetUserByEmailAndPassword(string email, string password);
	    User GetUserByEmail(string email);
	    void UpdateLastLoginDate(string email);
	    IQueryable<UserReference> GetUsersAsReference();
	}

    
}