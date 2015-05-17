using System;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
	public class ProjectManager : IProjectManager
	{
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMessenger _messenger;
        private readonly IGeneralUnitOfWork _generalUnitOfWork;
        private readonly IValidatorFactory _validationFactory;

        public ProjectManager(IGeneralUnitOfWork generalUnitOfWork, IMessenger messenger, IValidatorFactory validationFactory)
        {
            _generalUnitOfWork = generalUnitOfWork;
            _messenger = messenger;
            _validationFactory = validationFactory;
        }
		
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