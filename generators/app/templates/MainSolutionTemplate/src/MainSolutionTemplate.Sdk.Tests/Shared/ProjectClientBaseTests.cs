using System;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public abstract class ProjectClientBaseTests : IntegrationTestsBase
    {
        private IProjectControllerActions _projectHubClient;
        private IProjectControllerStandardLookups _projectStandardLookups;

        protected abstract void Setup();

        protected void SetRequiredData(IProjectControllerActions projectControllerActions, IProjectControllerStandardLookups projectStandardLookups)
        {
            _projectHubClient = projectControllerActions;
            _projectStandardLookups = projectStandardLookups;
        }


        [Test]
        public void Get_WhenCalled_ShouldHaveStatusCodeOk()
        {
            // arrange
            Setup();
            // action
            var projectModels = _projectStandardLookups.Get().Result;
            // assert
            projectModels.Should().NotBeNull();
            projectModels.Count.Should().BeGreaterThan(0);
        }
        
        [Test]
        public void Get_WhenCalledWithGuild_ShouldLookupProject()
        {
            // arrange
            Setup();
            // action
            var projectModels = _projectHubClient.Get(Guid.NewGuid()).Result;
            // assert
            projectModels.Should().BeNull();
        }

        [Test]
        public void Get_WhenCalledWithValidGuild_ShouldLookupProject()
        {
            // arrange
            Setup();
            var projectId = _projectStandardLookups.Get().Result.ToArray().Select(x => x.Id).First();
            
            // action
            var projectModels = _projectHubClient.Get(projectId);
            // assert
            projectModels.Should().NotBeNull();
        }

      
        [Test]
        public void PostPutDelete_WhenWhenGivenValidModel_ShouldLookupModels()
        {
            // arrange
            Setup();
            var projectModel = Builder<ProjectModel>.CreateListOfSize(2).All().With(x=>x.Name = GetRandom.FirstName()).Build();
            
            // action
            var projectModels = _projectHubClient.Post(projectModel[0]).Result;
            var savedProject = _projectHubClient.Get(projectModels.Id).Result;
            var projectModelLoad = _projectHubClient.Put(projectModels.Id, projectModel[1]).Result;
            var removed = _projectHubClient.Delete(projectModels.Id).Result;
            var removedSecond = _projectHubClient.Delete(projectModels.Id).Result;
            var removedProject = _projectHubClient.Get(projectModels.Id).Result;

            // assert
            savedProject.Should().NotBeNull();
            removedProject.Should().BeNull();
            projectModel[0].Name.Should().Be(projectModels.Name);
            projectModel[1].Name.Should().Be(projectModelLoad.Name);
            removed.Should().BeTrue();
            savedProject.Should().NotBeNull();
            removedSecond.Should().BeFalse();
            
        }
    }
}