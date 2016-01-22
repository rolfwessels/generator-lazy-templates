using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.DataIntegrity;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Core.Tests.Managers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.DataIntegrity
{
    [TestFixture]
    public class DataIntegrityManagerIntegrationTests : BaseManagerTests
    {
        private IUserManager _userManager;
        private IProjectManager _projectManager;

        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            _userManager = IocApi.Instance.Resolve<IUserManager>();
            _projectManager = IocApi.Instance.Resolve<IProjectManager>();
        }

        #endregion
        
        [Test]
        public async Task UpdateAllReferences_GivenObject_ShouldUpdateTheReferences()   
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().WithValidData().Build();
            var user = Builder<User>.CreateNew().WithValidData().Build();
            user.DefaultProject = project.ToReference();

            await _projectManager.Insert(project);
            await _userManager.Insert(user);

            // action
            project.Name = "NewName";
            await _projectManager.Update(project);
            var userFound = await _userManager.Get(user.Id);
            await _userManager.Delete(user.Id);
            await _projectManager.Delete(project.Id);

            // assert
            userFound.DefaultProject.Name.Should().Be(project.Name);

            
        }
 
        [Test]
        public async Task UpdateAllReferences_GivenObject_ShouldStopRemovalOfObject()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().WithValidData().Build();
            var user = Builder<User>.CreateNew().WithValidData().Build();
            user.DefaultProject = project.ToReference();

            await _projectManager.Insert(project);
            await _userManager.Insert(user);

            // action
            Exception ex = null;
            try
            {
                _projectManager.Delete(project.Id).Wait();
            }
            catch (Exception e)
            {
                ex = e;
            }
            await _userManager.Delete(user.Id);
            await _projectManager.Delete(project.Id);

            // assert
            ex.Dump("ex");
            ex.ToFirstExceptionOfException().Should().BeOfType<Exception>();
            ex.ToFirstExceptionOfException().Message.Should().Be("Could not remove Project [Project: Name1]. It is currently referenced in 1 other data object.");
        }


       
    }
}