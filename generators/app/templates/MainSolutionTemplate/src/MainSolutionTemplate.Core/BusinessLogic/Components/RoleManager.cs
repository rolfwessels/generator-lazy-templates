using System.Linq;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class RoleManager : BaseManager<Role>, IRoleManager
    {
        public RoleManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
        }

        protected override IRepository<Role> Repository
        {
            get { return _generalUnitOfWork.Roles; }
        }

        #region IRoleManager Members

        public Role GetRoleByName(string name)
        {
            return _generalUnitOfWork.Roles.FindOne(x => x.Name == name).Result;
        }

        #endregion
    }
}