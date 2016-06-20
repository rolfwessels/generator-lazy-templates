using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class ProjectManager : BaseManager<Project>, IProjectManager
    {
        public ProjectManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
        }

        #region Overrides of BaseManager<Project>

        protected override IRepository<Project> Repository
        {
            get { return _generalUnitOfWork.Projects; }
        }

        #endregion
    }
}

/* scaffolding [
    {
      "FileName": "IocCoreBase.cs",
      "Indexline": "As<IUserManager>",
      "InsertAbove": false,
      "InsertInline": false,
      "Lines": [
        "builder.RegisterType<ProjectManager>().As<IProjectManager>();"
      ]
    },
    {
      "FileName": "MongoGeneralUnitOfWork.cs",
      "Indexline": "Projects = ",
      "InsertAbove": false,
      "InsertInline": false,
      "Lines": [
        "Projects = new MongoRepository<Project>(database);"
      ]
    },
    {
      "FileName": "IGeneralUnitOfWork.cs",
      "Indexline": "Projects",
      "InsertAbove": false,
      "InsertInline": false,
      "Lines": [
        "IRepository<Project> Projects { get; }"
      ]
    },
    {
      "FileName": "MongoGeneralUnitOfWork.cs",
      "Indexline": "Projects { get; private set; }",
      "InsertAbove": false,
      "InsertInline": false,
      "Lines": [
        "public IRepository<Project> Projects { get; private set; }"
      ]
    },
  {
      "FileName": "FakeGeneralUnitOfWork.cs",
      "Indexline": "Projects =",
      "InsertAbove": false,
      "InsertInline": false,
      "Lines": [
        "Projects = new FakeRepository<Project>();"
      ]
    },
    {
      "FileName": "FakeGeneralUnitOfWork.cs",
      "Indexline": "Projects { get",
      "InsertAbove": false,
      "InsertInline": false,
      "Lines": [
        "public IRepository<Project> Projects { get; private set; }"
      ]
    }
] scaffolding */