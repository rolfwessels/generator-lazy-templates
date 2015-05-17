using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class ProjectManagerTests : BaseManagerTests
    {
        private ProjectManager _systemManager;

        #region Overrides of SystemManagerTests

        public override void Setup()
        {
            base.Setup();
            _systemManager = new ProjectManager(_baseManagerArguments);
        }

        #endregion

        [Test]
        public void GetProjects_WhenCalled_ShouldReturnProjects()
        {
            // arrange
            Setup();
            const int expected = 2;
            _fakeGeneralUnitOfWork.Projects.AddFake(expected);
            // action
            var result = _systemManager.GetProjects();
            // assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public void GetProject_WhenCalledWithId_ShouldReturnSingleProject()
        {
            // arrange
            Setup();
            var addFake = _fakeGeneralUnitOfWork.Projects.AddFake();
            var guid = addFake.First().Id;
            // action
            var result = _systemManager.GetProject(guid);
            // assert
            result.Id.Should().Be(guid);
        }

        [Test]
        public void SaveProject_WhenCalledWithProject_ShouldSaveTheProject()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            // action
            var result = _systemManager.SaveProject(project);
            // assert
            _fakeGeneralUnitOfWork.Projects.Should().HaveCount(1);
            result.Should().NotBeNull();
        }

        [Test]
        public void SaveProject_WhenCalledWithProject_ShouldToLowerTheEmail()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            // action
            var result = _systemManager.SaveProject(project);
            // assert
            result.Id.Should().Be(project.Id);
        }

        [Test]
        public void SaveProject_WhenCalledWithProject_ShouldCallMessageThatDataWasInserted()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            // action
            _systemManager.SaveProject(project);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Project>>(m=>m.UpdateType == UpdateTypes.Inserted)),Times.Once);
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Project>>(m => m.UpdateType == UpdateTypes.Updated)), Times.Never);
        }


        [Test]
        public void SaveProject_WhenCalledWithExistingProject_ShouldCallMessageThatDataWasUpdated()
        {
            // arrange
            Setup();
            var project = _fakeGeneralUnitOfWork.Projects.AddFake().First();
            // action
            _systemManager.SaveProject(project);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Project>>(m=>m.UpdateType == UpdateTypes.Updated)),Times.Once);
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Project>>(m => m.UpdateType == UpdateTypes.Inserted)), Times.Never);
        }

        [Test]
        public void DeleteProject_WhenCalledWithExistingProject_ShouldCallMessageThatDataWasRemoved()
        {
            // arrange
            Setup();
            var project = _fakeGeneralUnitOfWork.Projects.AddFake().First();
            // action
            _systemManager.DeleteProject(project.Id);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Project>>(m=>m.UpdateType == UpdateTypes.Removed)),Times.Once);
            _systemManager.GetProject(project.Id).Should().BeNull();
        }
        
        

    }
}