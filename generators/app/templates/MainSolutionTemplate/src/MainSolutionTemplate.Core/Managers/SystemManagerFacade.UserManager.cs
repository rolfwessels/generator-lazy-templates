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
				_log.Info(string.Format("Adding user [{0}]", user));
				_generalUnitOfWork.Users.Add(user);
				return user;
			}
			else
			{
				_log.Info(string.Format("Update user [{0}]", user));
				_generalUnitOfWork.Users.Update(user);
				
			}
			return user;
		}

		public User DeleteUser(Guid id)
		{
			var user = _generalUnitOfWork.Users.FirstOrDefault(x => x.Id == id);
			if (user != null)
			{
				_log.Info(string.Format("Remove user [{0}]", user));
				_generalUnitOfWork.Users.Remove(user);
			}
			return user;
		}
	}
}