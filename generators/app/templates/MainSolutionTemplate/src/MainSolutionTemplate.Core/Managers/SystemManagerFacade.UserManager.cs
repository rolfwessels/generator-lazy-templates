using System;
using System.Linq;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManager : IUserManager
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
				_messenger.Send(new DalUpdateMessage<User>(user, Types.Inserted));
				return user;
			}
			else
			{
				_log.Info(string.Format("Update user [{0}]", user));
				_generalUnitOfWork.Users.Update(user);
				_messenger.Send(new DalUpdateMessage<User>(user, Types.Updated));
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
				_messenger.Send(new DalUpdateMessage<User>(user, Types.Removed));
			}
			return user;
		}

		public User GetUserByEmailAndPassword(string email, string password)
		{
			User user = GetUserByEmail(email);
			if (user != null && user.HashedPassword != null)
			{
				if (!PasswordHash.ValidatePassword(password, user.HashedPassword))
				{
					user = null;
					_log.Info(string.Format("Invalid password for user '{0}'", email));
				}
			}
			else
			{
				_log.Info(string.Format("Invalid user '{0}'", email));
			}
			return user;
		}

		public User GetUserByEmail(string email)
		{
			return _generalUnitOfWork.Users.FirstOrDefault(x => x.Email == email);
		}
	}
}