using System;
using FizzWare.NBuilder;
using MainSolutionTemplate.Core.Tests.Managers;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Core.Tests
{
	[TestFixture]
	public class SystemManagerProjectManagerTests : SystemManagerTests
	{
		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
            _systemManager.Should().NotBeNull();
		}

	    [Test]
        public void GetProjects_WhenCalled_ShouldReturnProjects()
	    {
	        // arrange
	        Setup();
	        var project = Builder<Project>.CreateNew().Build();
	        _fakeGeneralUnitOfWork.Projects.Add(project);
	        // action
	        var projects = _systemManager.GetProjects();
	        // assert
	        projects.Should().Contain(project);
	    }
  
        [Test]
        public void GetProject_WhenCalled_ShouldReturnProjects()
	    {
	        // arrange
	        Setup();
	        var projectAdd = Builder<Project>.CreateNew().Build();
	        _fakeGeneralUnitOfWork.Projects.Add(projectAdd);
	        // action
            var project = _systemManager.GetProject(projectAdd.Id);
	        // assert
            project.Id.Should().Be(projectAdd.Id);
	    }


        [Test]
        public void SaveProject_WhenCalled_ShouldAddProjects()
	    {
	        // arrange
	        Setup();
	        var projectAdd = Builder<Project>.CreateNew().Build();
	        // action
            var project = _systemManager.SaveProject(projectAdd);
	        // assert
            _fakeGeneralUnitOfWork.Projects.Should().Contain(project);
	    }


	    [Test]
	    public void SaveProject_WhenCalledWithInvalidData_ShouldNotAddProjectsAndThrowException()
	    {
	        // arrange
	        Setup();
	        var projectAdd = Builder<Project>.CreateNew().Build();
	        _mockIValidatorFactory.Setup(mc => mc.ValidateAndThrow(projectAdd))
                                  .Throws(new Exception("Where is the Name"));
	        // action
	        Action testCall = () => { _systemManager.SaveProject(projectAdd); };
	        // assert
	        testCall.ShouldThrow<Exception>().WithMessage("Where is the Name");
	        _fakeGeneralUnitOfWork.Projects.Should().NotContain(projectAdd);
	    }

	    [Test]
        public void DeleteProject_WhenCalledWithExistingItem_ShouldRemoveThatProject()
	    {
	        // arrange
	        Setup();
            var projectAdd = Builder<Project>.CreateNew().Build();
            _fakeGeneralUnitOfWork.Projects.Add(projectAdd);
	        // action
	        var deleteProject = _systemManager.DeleteProject(projectAdd.Id);
	        // assert
	        deleteProject.Should().NotBeNull();
	    }
        
        [Test]
        public void DeleteProject_WhenCalledNonWithExistingItem_ShouldRemoveThatProject()
	    {
	        // arrange
	        Setup();
            var projectAdd = Builder<Project>.CreateNew().Build();
	        // action
	        var deleteProject = _systemManager.DeleteProject(projectAdd.Id);
	        // assert
	        deleteProject.Should().BeNull();
	    }

	}
}