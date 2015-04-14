using System;
using System.Linq;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;

namespace MainSolutionTemplate.Core.Managers.Interfaces
{
	public interface IProjectManager
	{
		IQueryable<Project> GetProjects();
        IQueryable<ProjectReference> GetProjectsAsReference();
		Project GetProject(Guid id);
        Project SaveProject(Project project);
		Project DeleteProject(Guid id);
	}

    
}