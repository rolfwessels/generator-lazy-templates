using FizzWare.NBuilder;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class ApplicationManagerTests : BaseTypedManagerTests<Application>
    {
        private ApplicationManager _userManager;

        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            _userManager = new ApplicationManager(_baseManagerArguments);
        }

        #endregion

        protected override IRepository<Application> Repository
        {
            get { return _fakeGeneralUnitOfWork.Applications; }
        }

        protected override Application SampleObject
        {
            get { return Builder<Application>.CreateNew().Build(); }
        }

        protected override BaseManager<Application> Manager
        {
            get { return _userManager; }
        }
    }
}