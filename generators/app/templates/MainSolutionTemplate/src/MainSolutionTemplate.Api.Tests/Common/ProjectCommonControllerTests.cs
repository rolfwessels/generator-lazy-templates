using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Models;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.Common
{
    [TestFixture]
    public class ProjectCommonControllerTests
    {

        private ProjectCommonController _projectCommonController;
        private Mock<IProjectManager> _mockISystemManager;

        #region Setup/Teardown

        public void Setup()
        {
            _mockISystemManager = new Mock<IProjectManager>(MockBehavior.Strict);
            _projectCommonController = new ProjectCommonController(_mockISystemManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockISystemManager.VerifyAll();
        }

        #endregion

        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _projectCommonController.Should().NotBeNull();
        }

        [Test]
        public void Get_GivenRequest_ShouldReturnProjectReferenceModels()
        {
            // arrange
            Setup();
            var projectReference = Builder<Project>.CreateListOfSize(2).Build();
            _mockISystemManager.Setup(mc => mc.GetProjects())
                .Returns(projectReference.AsQueryable);
            // action
            var result = _projectCommonController.Get().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void GetDetail_GivenRequest_ShouldReturnProjectModel()
        {
            // arrange
            Setup();
            var projectReference = Builder<Project>.CreateListOfSize(2).Build();
            _mockISystemManager.Setup(mc => mc.GetProjects())
                .Returns(projectReference.AsQueryable);
            // action
            var result = _projectCommonController.GetDetail().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void Get_GivenProjectId_ShouldCallGetProject()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.GetProject(project.Id))
                .Returns(project);
            // action
            var result = _projectCommonController.Get(project.Id).Result;
            // assert
            result.Id.Should().Be(project.Id);
        }

        [Test]
        public void Put_GivenProjectId_ShouldUpdateAGivenProject()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.GetProject(project.Id))
                .Returns(project);
            _mockISystemManager.Setup(mc => mc.SaveProject(project))
                .Returns(project);
            var projectDetailModel = new ProjectDetailModel();
            // action
            var result = _projectCommonController.Put(project.Id, projectDetailModel).Result;
            // assert
            result.Id.Should().Be(project.Id);
        }

        [Test]
        public void Post_GivenProjectId_ShouldAddAProject()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            var projectDetailModel = Builder<ProjectDetailModel>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.SaveProject(It.Is<Project>(project1 => project1.Name == project.Name))).Returns(project);
            // action
            var result = _projectCommonController.Post(projectDetailModel).Result;
            // assert
            result.Id.Should().Be(project.Id);
        }


        [Test]
        public void Delete_GivenProjectId_ShouldRemoveProject()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.DeleteProject(project.Id)).Returns(project);
            // action
            var result = _projectCommonController.Delete(project.Id).Result;
            // assert
            result.Should().Be(true);
        }



         
    }

}