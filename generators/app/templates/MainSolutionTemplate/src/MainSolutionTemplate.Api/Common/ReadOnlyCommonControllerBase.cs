using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MainSolutionTemplate.Api.WebApi.ODataSupport;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;

namespace MainSolutionTemplate.Api.Common
{
    public abstract class ReadOnlyCommonControllerBase<TDal, TModel, TReferenceModel>
    {
        protected IBaseManager<TDal> _projectManager;

        public Task<IEnumerable<TReferenceModel>> Get(string query = null)
        {
            return
                Task.Run(
                    () =>
                    new QueryToODataWrapper<TDal, TReferenceModel>(_projectManager, query, ToReferenceModelList)
                    as IEnumerable<TReferenceModel>);
        }

        public Task<IEnumerable<TModel>> GetDetail(string query = null)
        {
            return Task.Run(() => new QueryToODataWrapper<TDal, TModel>(_projectManager, query, ToModelList) as IEnumerable<TModel>);
        }

        public Task<TModel> Get(Guid id)
        {
            return Task.Run(() => ToModel(_projectManager.Get(id)));
        }

        protected virtual TModel ToModel(TDal arg)
        {
            return Mapper.Map<TDal, TModel>(arg);
        }

        protected virtual IEnumerable<TModel> ToModelList(IEnumerable<TDal> arg)
        {
            return Mapper.Map<IEnumerable<TDal>, IEnumerable<TModel>>(arg);
        }

        protected virtual IEnumerable<TReferenceModel> ToReferenceModelList(IEnumerable<TDal> arg)
        {
            return Mapper.Map<IEnumerable<TDal>, IEnumerable<TReferenceModel>>(arg);
        }
    }
}