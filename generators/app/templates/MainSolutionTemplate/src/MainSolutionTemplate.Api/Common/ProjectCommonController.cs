using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.WebApi.ODataSupport;
using MainSolutionTemplate.Core.BusinessLogic.Facade.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using log4net;

namespace MainSolutionTemplate.Api.Common
{
    public class ProjectCommonController : IProjectControllerActions
    {
        private readonly ISystemManager _systemManager;

        public ProjectCommonController(ISystemManager systemManager)
        {
            _systemManager = systemManager;
            
        }
        public Task<IEnumerable<ProjectReferenceModel>> Get(string query = null)
        {
            return Task.Run(() => new QueryToODataWrapper<Project, ProjectReferenceModel>(_systemManager.GetProjects(), query, MapApi.ToReferenceModelList) as IEnumerable<ProjectReferenceModel>);
        }

        public Task<IEnumerable<ProjectModel>> GetDetail(string query = null)
        {
            return Task.Run(() => new QueryToODataWrapper<Project, ProjectModel>(_systemManager.GetProjects(), query, MapApi.ToModelList) as IEnumerable<ProjectModel>);
        }

        public Task<ProjectModel> Get(Guid id)
        {
            return Task.Run(() => _systemManager.GetProject(id).ToModel());
        }

        public Task<ProjectModel> Put(Guid id, ProjectDetailModel project)
        {
            return Task.Run(() =>
            {
                var projectFound = _systemManager.GetProject(id);
                if (projectFound == null) throw new Exception(string.Format("Could not find model by id '{0}'", id));
                var saveProject = _systemManager.SaveProject(project.ToDal(projectFound));
                return saveProject.ToModel();
            });
        }

       
        public Task<ProjectModel> Post(ProjectDetailModel project)
        {
            return Task.Run(() =>
            {
                var savedProject = _systemManager.SaveProject(project.ToDal());
                return savedProject.ToModel();
            });
        }

        public Task<bool> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                var deleteProject = _systemManager.DeleteProject(id);
                return deleteProject != null;
            });
        }

    }

   
}