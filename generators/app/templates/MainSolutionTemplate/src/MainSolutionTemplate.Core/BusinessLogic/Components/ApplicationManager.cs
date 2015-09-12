using System.Linq;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class ApplicationManager : BaseManager<Application>, IApplicationManager
    {
        public ApplicationManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
        }

        protected override IRepository<Application> Repository
        {
            get { return _generalUnitOfWork.Applications; }
        }

        #region IApplicationManager Members

        public Application GetApplicationById(string clientId)
        {
            return _generalUnitOfWork.Applications.FindOne(x => x.ClientId == clientId).Result;
        }

        #endregion
    }
}