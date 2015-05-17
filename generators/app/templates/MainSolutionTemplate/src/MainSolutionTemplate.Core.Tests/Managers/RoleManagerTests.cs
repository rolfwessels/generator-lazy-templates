using FizzWare.NBuilder;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class RoleManagerTests : BaseTypedManagerTests<Role>
    {
        private RoleManager _roleManager;

        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            _roleManager = new RoleManager(_baseManagerArguments);
        }

        #endregion

        protected override IRepository<Role> Repository
        {
            get { return _fakeGeneralUnitOfWork.Roles; }
        }

        protected override Role SampleObject
        {
            get { return Builder<Role>.CreateNew().Build(); }
        }

        protected override BaseManager<Role> Manager
        {
            get { return _roleManager; }
        }
    }
}