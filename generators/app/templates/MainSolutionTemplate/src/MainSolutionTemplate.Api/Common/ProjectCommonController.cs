using System.Linq;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.Common
{
    public class ProjectCommonController : BaseCommonController<Project, ProjectModel, ProjectReferenceModel, ProjectDetailModel>, IProjectControllerActions
    {
        
        public ProjectCommonController(IProjectManager projectManager)
            : base(projectManager)
        {
        }

    }
}