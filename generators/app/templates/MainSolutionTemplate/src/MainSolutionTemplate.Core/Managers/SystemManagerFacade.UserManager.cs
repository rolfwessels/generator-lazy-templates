using System;
using System.Linq;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManagerFacade : IUserManager
	{
		
		public IQueryable<User> GetUsers()
		{
			return _generalUnitOfWork.Users;
		}

		public User GetUser(Guid id)
		{
			return _generalUnitOfWork.Users.FirstOrDefault(x => x.Id == id);
		}

		public User SaveUser(User user)
		{
			var userFound = _generalUnitOfWork.Users.FirstOrDefault(x => x.Id == user.Id);
			if (userFound == null)
			{
				_generalUnitOfWork.Users.Add(user);
			}
			else
			{
				_generalUnitOfWork.Users.Update(userFound);
			}

			return userFound;
		}

		public User DeleteUser(Guid id)
		{
			var user = _generalUnitOfWork.Users.FirstOrDefault(x => x.Id == id);
			if (user != null)
			{
				_generalUnitOfWork.Users.Remove(user);
			}
			return user;
		}
	}
}