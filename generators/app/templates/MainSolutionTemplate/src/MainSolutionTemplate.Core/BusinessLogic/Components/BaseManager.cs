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
    public abstract class BaseManager
    {
        protected readonly IGeneralUnitOfWork _generalUnitOfWork;
        protected readonly IMessenger _messenger;
        protected readonly IValidatorFactory _validationFactory;

        protected BaseManager(BaseManagerArguments baseManagerArguments)
        {
            _generalUnitOfWork = baseManagerArguments.GeneralUnitOfWork;
            _messenger = baseManagerArguments.Messenger;
            _validationFactory = baseManagerArguments.ValidationFactory;
        }
    }

    public abstract class BaseManager<T> : BaseManager, IBaseManager<T> where T : BaseDalModelWithId
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _name;

        protected BaseManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
            _name = typeof(T).Name;
        }
        
        public virtual IQueryable<T> GetProjects()
        {
            return Repository;
        }

        public virtual T GetProject(Guid id)
        {

            return Repository.FirstOrDefault(x => x.Id == id);
        }

        public virtual  T SaveProject(T project)
        {
            var projectFound = Repository.FirstOrDefault(x => x.Id == project.Id);
            _validationFactory.ValidateAndThrow(project);
            if (projectFound == null)
            {
                _log.Info(string.Format("Adding {1} [{0}]", project, _name));
                Repository.Add(project);
                _messenger.Send(new DalUpdateMessage<T>(project, UpdateTypes.Inserted));
                return project;
            }
            _log.Info(string.Format("Update {1} [{0}]", project, _name));
            Repository.Update(project);
            _messenger.Send(new DalUpdateMessage<T>(project, UpdateTypes.Updated));
            return project;

        }

        public virtual T DeleteProject(Guid id)
        {
            var project = Repository.FirstOrDefault(x => x.Id == id);
            if (project != null)
            {
                _log.Info(string.Format("Remove {1} [{0}]", project,_name));
                Repository.Remove(project);
                _messenger.Send(new DalUpdateMessage<T>(project, UpdateTypes.Removed));
            }
            return project;
        }

        protected abstract IRepository<T> Repository { get; }
    }
}