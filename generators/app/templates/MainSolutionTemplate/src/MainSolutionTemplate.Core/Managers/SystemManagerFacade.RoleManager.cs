using System;
using System.Linq;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManager : IRoleManager
	{
		#region IRoleManager Members

		public IQueryable<Role> GetRoles()
		{
			return _generalUnitOfWork.Roles;
		}

		public Role GetRole(Guid id)
		{
			return _generalUnitOfWork.Roles.FirstOrDefault(x => x.Id == id);
		}

		public Role GetRoleByName(string name)
		{
			return _generalUnitOfWork.Roles.FirstOrDefault(x => x.Name == name);
		}

		public Role SaveRole(Role role)
		{
			Role roleFound = _generalUnitOfWork.Roles.FirstOrDefault(x => x.Id == role.Id);
			if (roleFound == null)
			{
				_log.Info(string.Format("Adding role [{0}]", role));
				_generalUnitOfWork.Roles.Add(role);
				_messenger.Send(new DalUpdateMessage<Role>(role, UpdateTypes.Inserted));
				return role;
			}
			else
			{
				_log.Info(string.Format("Update role [{0}]", role));
				_generalUnitOfWork.Roles.Update(role);
				_messenger.Send(new DalUpdateMessage<Role>(role, UpdateTypes.Updated));
			}
			return role;
		}

		public Role DeleteRole(Guid id)
		{
			Role role = _generalUnitOfWork.Roles.FirstOrDefault(x => x.Id == id);
			if (role != null)
			{
				_log.Info(string.Format("Remove role [{0}]", role));
				_generalUnitOfWork.Roles.Remove(role);
				_messenger.Send(new DalUpdateMessage<Role>(role, UpdateTypes.Removed));
			}
			return role;
		}

		#endregion

	   
	}
}