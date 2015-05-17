using System;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class RoleManager : BaseManager, IRoleManager
	{
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public RoleManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
        }

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