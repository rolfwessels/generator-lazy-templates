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