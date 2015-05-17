using System;
using System.Linq;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
	public interface IRoleManager : IBaseManager<Role>
	{
		Role GetRoleByName(string name);
	}

	
}