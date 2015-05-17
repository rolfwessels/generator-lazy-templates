using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.WebApi.ODataSupport;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.Common
{
    public class ProjectCommonController : IProjectControllerActions
    {
        private readonly IProjectManager _projectManager;

        public ProjectCommonController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
            
        }

        public Task<IEnumerable<ProjectReferenceModel>> Get(string query = null)
        {
            return Task.Run(() => new QueryToODataWrapper<Project, ProjectReferenceModel>(_projectManager.Get(), query, MapApi.ToReferenceModelList) as IEnumerable<ProjectReferenceModel>);
        }

        public Task<IEnumerable<ProjectModel>> GetDetail(string query = null)
        {
            return Task.Run(() => new QueryToODataWrapper<Project, ProjectModel>(_projectManager.Get(), query, MapApi.ToModelList) as IEnumerable<ProjectModel>);
        }

        public Task<ProjectModel> Get(Guid id)
        {
            return Task.Run(() => _projectManager.Get(id).ToModel());
        }

        public Task<ProjectModel> Put(Guid id, ProjectDetailModel model)
        {
            return Task.Run(() =>
            {
                var projectFound = _projectManager.Get(id);
                if (projectFound == null) throw new Exception(string.Format("Could not find model by id '{0}'", id));
                var saveProject = _projectManager.Save(model.ToDal(projectFound));
                return saveProject.ToModel();
            });
        }

       
        public Task<ProjectModel> Post(ProjectDetailModel model)
        {
            return Task.Run(() =>
            {
                var savedProject = _projectManager.Save(model.ToDal());
                return savedProject.ToModel();
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

    }

   
}