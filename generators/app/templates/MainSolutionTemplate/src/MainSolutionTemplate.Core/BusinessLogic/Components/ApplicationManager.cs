using System;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
	public class ApplicationManager : IApplicationManager
	{
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMessenger _messenger;
        private readonly IGeneralUnitOfWork _generalUnitOfWork;
        private readonly IValidatorFactory _validationFactory;

        public ApplicationManager(IGeneralUnitOfWork generalUnitOfWork, IMessenger messenger, IValidatorFactory validationFactory)
        {
            _generalUnitOfWork = generalUnitOfWork;
            _messenger = messenger;
            _validationFactory = validationFactory;
        }
		
		public IQueryable<Application> GetApplications()
		{
			return _generalUnitOfWork.Applications;
		}

	    
	    public Application GetApplication(Guid id)
		{
           
			return _generalUnitOfWork.Applications.FirstOrDefault(x => x.Id == id);
		}

	    public Application GetApplicationById(string clientId)
	    {
            return _generalUnitOfWork.Applications.FirstOrDefault(x => x.ClientId == clientId);
	    }

	    public Application SaveApplication(Application project)
		{
		    var projectFound = _generalUnitOfWork.Applications.FirstOrDefault(x => x.Id == project.Id);
		    _validationFactory.ValidateAndThrow(project);
		    if (projectFound == null)
			{
				_log.Info(string.Format("Adding project [{0}]", project));
				_generalUnitOfWork.Applications.Add(project);
				_messenger.Send(new DalUpdateMessage<Application>(project, UpdateTypes.Inserted));
				return project;
			}
		    _log.Info(string.Format("Update project [{0}]", project));
		    _generalUnitOfWork.Applications.Update(project);
		    _messenger.Send(new DalUpdateMessage<Application>(project, UpdateTypes.Updated));
		    return project;
		}

		public Application DeleteApplication(Guid id)
		{
			var project = _generalUnitOfWork.Applications.FirstOrDefault(x => x.Id == id);
			if (project != null)
			{
				_log.Info(string.Format("Remove project [{0}]", project));
				_generalUnitOfWork.Applications.Remove(project);
				_messenger.Send(new DalUpdateMessage<Application>(project, UpdateTypes.Removed));
			}
			return project;
		}

	}

    
}