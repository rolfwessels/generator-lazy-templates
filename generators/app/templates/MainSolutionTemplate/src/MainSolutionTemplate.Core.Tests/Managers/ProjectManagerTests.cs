using FizzWare.NBuilder;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class ProjectManagerTests : BaseTypedManagerTests<Project>
    {
        private ProjectManager _projectManager;

        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            _projectManager = new ProjectManager(_baseManagerArguments);
        }

        #endregion

        protected override IRepository<Project> Repository
        {
            get { return _fakeGeneralUnitOfWork.Projects; }
        }

        protected override Project SampleObject
        {
            get { return Builder<Project>.CreateNew().Build(); }
        }

        protected override BaseManager<Project> Manager
        {
            get { return _projectManager; }
        }
    }
}