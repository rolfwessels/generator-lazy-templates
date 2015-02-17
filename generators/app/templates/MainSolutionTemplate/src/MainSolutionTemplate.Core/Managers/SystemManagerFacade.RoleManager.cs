using System;
using System.Linq;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManagerFacade : IRoleManager
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
				Messenger.Default.Send(new DalUpdateMessage<Role>(role, Types.Inserted));
				return role;
			}
			else
			{
				_log.Info(string.Format("Update role [{0}]", role));
				_generalUnitOfWork.Roles.Update(role);
				Messenger.Default.Send(new DalUpdateMessage<Role>(role, Types.Updated));
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
				Messenger.Default.Send(new DalUpdateMessage<Role>(role, Types.Removed));
			}
			return role;
		}

		#endregion
	}
}