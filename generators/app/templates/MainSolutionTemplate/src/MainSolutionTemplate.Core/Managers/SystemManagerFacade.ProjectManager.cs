using System;
using System.Linq;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Dal.Validation;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManager : IProjectManager
	{
		
		public IQueryable<Project> GetProjects()
		{
			return _generalUnitOfWork.Projects;
		}

	    public IQueryable<ProjectReference> GetProjectsAsReference()
	    {
            return _generalUnitOfWork.Projects.Select(x => new ProjectReference { Id = x.Id, Name = x.Name });
	    }

	    public Project GetProject(Guid id)
		{
           
			return _generalUnitOfWork.Projects.FirstOrDefault(x => x.Id == id);
		}

		public Project SaveProject(Project project)
		{
		    var projectFound = _generalUnitOfWork.Projects.FirstOrDefault(x => x.Id == project.Id);
		    _validationFactory.ValidateAndThrow(project);
		    if (projectFound == null)
			{
				_log.Info(string.Format("Adding project [{0}]", project));
				_generalUnitOfWork.Projects.Add(project);
				_messenger.Send(new DalUpdateMessage<Project>(project, UpdateTypes.Inserted));
				return project;
			}
		    _log.Info(string.Format("Update project [{0}]", project));
		    _generalUnitOfWork.Projects.Update(project);
		    _messenger.Send(new DalUpdateMessage<Project>(project, UpdateTypes.Updated));
		    return project;
		}

		public Project DeleteProject(Guid id)
		{
			var project = _generalUnitOfWork.Projects.FirstOrDefault(x => x.Id == id);
			if (project != null)
			{
				_log.Info(string.Format("Remove project [{0}]", project));
				_generalUnitOfWork.Projects.Remove(project);
				_messenger.Send(new DalUpdateMessage<Project>(project, UpdateTypes.Removed));
			}
			return project;
		}

	}
}