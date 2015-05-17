using System;
using System.Threading.Tasks;
using AutoMapper;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Api.Common
{
    public abstract class BaseCommonController<TDal, TModel, TReferenceModel, TDetailModel> : ReadOnlyCommonControllerBase<TDal, TModel, TReferenceModel>, ICrudController<TModel, TDetailModel>
    {
        protected BaseCommonController()
        {
        }

        protected BaseCommonController(IBaseManager<TDal> projectManager)
        {
            _projectManager = projectManager;
        }


        public Task<TModel> Update(Guid id, TDetailModel model)
        {
            return Task.Run(() =>
                {
                    var projectFound = _projectManager.Get(id);
                    if (projectFound == null) throw new Exception(string.Format("Could not find model by id '{0}'", id));
                    var project = ToDal(model,projectFound);
                    var saveProject = _projectManager.Save(project);
                    return ToModel(saveProject);
                });
        }


        public Task<TModel> Insert(TDetailModel model)
        {
            return Task.Run(() =>
                {
                    var savedProject = _projectManager.Save(ToDal(model));
                    return ToModel(savedProject);
                });
        }

        public Task<bool> Delete(Guid id)
        {
            return Task.Run(() =>
                {
                    var deleteProject = _projectManager.Delete(id);
                    return deleteProject != null;
                });
        }

        protected virtual TDal ToDal(TDetailModel arg)
        {
            return Mapper.Map<TDetailModel, TDal>(arg);
        }
        
        protected virtual TDal ToDal(TDetailModel arg,TDal inp)
        {
            return Mapper.Map(arg, inp);
        }
    }
}