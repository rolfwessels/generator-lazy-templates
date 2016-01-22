using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.Common
{
    [TestFixture]
    public class ProjectCommonControllerTests : BaseCommonControllerTests<Project, ProjectModel, ProjectReferenceModel, ProjectCreateUpdateModel, IProjectManager>
    {
        private Mock<IProjectManager> _mockIProjectManager;
        private ProjectCommonController _projectCommonController;


        #region Overrides of BaseCommonControllerTests

        public override void Setup()
        {
            _mockIProjectManager = new Mock<IProjectManager>(MockBehavior.Strict);
            _projectCommonController = new ProjectCommonController(_mockIProjectManager.Object);
            base.Setup();
        }

        protected override Mock<IProjectManager> GetManager()
        {
            return _mockIProjectManager;
        }

        protected override BaseCommonController<Project, ProjectModel, ProjectReferenceModel, ProjectCreateUpdateModel> GetCommonController()
        {
            return _projectCommonController;
        }

        public override void TearDown()
        {
            base.TearDown();
            _mockIProjectManager.VerifyAll();
        }

        #endregion

    }
}