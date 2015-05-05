using System;
using System.Linq;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
	public interface IRoleManager
	{
		IQueryable<Role> GetRoles();
		Role GetRole(Guid id);
		Role GetRoleByName(string name);
		Role SaveRole(Role role);
		Role DeleteRole(Guid id);	
	}

	
}