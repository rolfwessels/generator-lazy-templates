using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MainSolutionTemplate.Api.WebApi.ODataSupport;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;

namespace MainSolutionTemplate.Api.Common
{
    public abstract class ReadOnlyCommonControllerBase<TDal, TModel, TReferenceModel>
    {
        protected IBaseManager<TDal> _projectManager;

        public async Task<IEnumerable<TReferenceModel>> Get(string query = null)
        {
            var dals = await _projectManager.Get();
            var queryToODataWrapper = new QueryToODataWrapper<TDal, TReferenceModel>(dals.AsQueryable(), query, ToReferenceModelList);
            return queryToODataWrapper as IEnumerable<TReferenceModel>;
        }

      

        public async  Task<IEnumerable<TModel>> GetDetail(string query = null)
        {
            var dals = await _projectManager.Get();
            var queryToODataWrapper = new QueryToODataWrapper<TDal, TModel>(dals.AsQueryable(), query, ToModelList);
            return queryToODataWrapper as IEnumerable<TModel>;
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