using System.Linq;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.Common
{
    public class ProjectCommonController : BaseCommonController<Project, ProjectModel, ProjectReferenceModel, ProjectCreateUpdateModel>, IProjectControllerActions
    {
        
        public ProjectCommonController(IProjectManager projectManager)
            : base(projectManager)
        {
        }

    }
}
/* scaffolding [
    {
      "FileName": "IocApi.cs",
      "Indexline": "RegisterType<ProjectCommonController>",
      "InsertAbove": false,
      "InsertInline": false,
      "Lines": [
        "builder.RegisterType<ProjectCommonController>();"
      ]
    }
] scaffolding */