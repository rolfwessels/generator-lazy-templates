using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
            _name = typeof (T).Name;
        }

        protected abstract IRepository<T> Repository { get; }

        #region IBaseManager<T> Members

      
        public Task<List<T>> Get(Expression<Func<T, bool>> filter)
        {
            return Repository.Find(filter);
        }

        public Task<List<T>> Get()
        {
            return Repository.Find(x => true);
        }

        public virtual Task<T> Get(Guid id)
        {
            return Repository.FindOne(x => x.Id == id);
        }

        public virtual async Task<T> Save(T entity)
        {
            T projectFound =  await Get(entity.Id);
            if (projectFound == null)
            {
                return await Insert(entity);
            }
            return await Update(entity); 
        }

        public virtual async Task<T> Delete(Guid id)
        {
            T project = await Get(id);
            if (project != null)
            {
                _log.Info(string.Format("Remove {1} [{0}]", project, _name));
                await Repository.Remove(x => x.Id == id);
                _messenger.Send(new DalUpdateMessage<T>(project, UpdateTypes.Removed));
            }
            return project;
        }

        public IQueryable<T> Query()
        {
            return Repository.Query();
        }

        #endregion

        protected async Task<T> Update(T entity)
        {
            DefaultModelNormalize(entity);
            await Validate(entity);
            _log.Info(string.Format("Update {1} [{0}]", entity, _name));
            T update = await Repository.Update(x => x.Id == entity.Id, entity);
            _messenger.Send(new DalUpdateMessage<T>(entity, UpdateTypes.Updated));
            return update;
        }

        

        protected async Task<T> Insert(T entity)
        {
            DefaultModelNormalize(entity);
            await Validate(entity);
            _log.Info(string.Format("Adding {1} [{0}]", entity, _name));
            var insert = await Repository.Add(entity);
            _messenger.Send(new DalUpdateMessage<T>(entity, UpdateTypes.Inserted));
            return insert;
        }

        protected virtual void DefaultModelNormalize(T user)
        {
        }

        protected virtual Task Validate(T entity)
        {
            _validationFactory.ValidateAndThrow(entity);
            return Task.FromResult(true);
        }
    }
}