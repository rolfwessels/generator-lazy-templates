using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Persistance;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class RoleManagerTests
    {
        private RoleManager _roleManager;

        #region Setup/Teardown

        public void Setup()
        {
            _roleManager = new RoleManager();
        }

        #endregion

        [Test]
        public void GetRoleByName_GivenAdminRole_ShouldReturn()
        {
            // arrange
            Setup();
            // action
            var roleByName = _roleManager.GetRoleByName("Admin").Result;
            // assert
            roleByName.Name.Should().Be("Admin");
            roleByName.Activities.Should().Contain(Activity.DeleteUser);
            roleByName.Activities.Should().NotBeEmpty();
        }

        [Test]
        public void GetRoleByName_GivenGuestRole_ShouldReturn()
        {
            // arrange
            Setup();
            // action
            var roleByName = _roleManager.GetRoleByName("Guest").Result;
            // assert
            roleByName.Name.Should().Be("Guest");
            roleByName.Activities.Should().NotContain(Activity.DeleteUser);
            roleByName.Activities.Should().NotBeEmpty();
        }

        [Test]
        public void GetRoleByName_GivenInvalidRole_ShouldReturnNull()
        {
            // arrange
            Setup();
            // action
            var roleByName = _roleManager.GetRoleByName("Guest123123").Result;
            // assert
            roleByName.Should().BeNull();

        }
    }
}