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
    public class RoleManagerTests : BaseManagerTests
    {
        private RoleManager _roleManager;

        #region Overrides of BaseManagerTests

        public override void Setup()
        {
            base.Setup();
            _roleManager = new RoleManager(_baseManagerArguments);
        }

        #endregion

        [Test]
        public void GetRoles_WhenCalled_ShouldReturnRoles()
        {
            // arrange
            Setup();
            const int expected = 2;
            _fakeGeneralUnitOfWork.Roles.AddFake(expected);
            // action
            var result = _roleManager.GetRoles();
            // assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public void GetRole_WhenCalledWithId_ShouldReturnSingleRole()
        {
            // arrange
            Setup();
            var addFake = _fakeGeneralUnitOfWork.Roles.AddFake();
            var guid = addFake.First().Id;
            // action
            var result = _roleManager.GetRole(guid);
            // assert
            result.Id.Should().Be(guid);
        }

        [Test]
        public void SaveRole_WhenCalledWithRole_ShouldSaveTheRole()
        {
            // arrange
            Setup();
            var role = Builder<Role>.CreateNew().Build();
            // action
            var result = _roleManager.SaveRole(role);
            // assert
            _fakeGeneralUnitOfWork.Roles.Should().HaveCount(1);
            result.Should().NotBeNull();
        }

        [Test]
        public void SaveRole_WhenCalledWithRole_ShouldToLowerTheEmail()
        {
            // arrange
            Setup();
            var role = Builder<Role>.CreateNew().Build();
            // action
            var result = _roleManager.SaveRole(role);
            // assert
            result.Name.Should().Be(role.Name);
        }

        [Test]
        public void SaveRole_WhenCalledWithRole_ShouldCallMessageThatDataWasInserted()
        {
            // arrange
            Setup();
            var role = Builder<Role>.CreateNew().Build();
            // action
            _roleManager.SaveRole(role);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Role>>(m=>m.UpdateType == UpdateTypes.Inserted)),Times.Once);
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Role>>(m => m.UpdateType == UpdateTypes.Updated)), Times.Never);
        }


        [Test]
        public void SaveRole_WhenCalledWithExistingRole_ShouldCallMessageThatDataWasUpdated()
        {
            // arrange
            Setup();
            var role = _fakeGeneralUnitOfWork.Roles.AddFake().First();
            // action
            _roleManager.SaveRole(role);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Role>>(m=>m.UpdateType == UpdateTypes.Updated)),Times.Once);
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Role>>(m => m.UpdateType == UpdateTypes.Inserted)), Times.Never);
        }

        [Test]
        public void DeleteRole_WhenCalledWithExistingRole_ShouldCallMessageThatDataWasRemoved()
        {
            // arrange
            Setup();
            var role = _fakeGeneralUnitOfWork.Roles.AddFake().First();
            // action
            _roleManager.DeleteRole(role.Id);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<Role>>(m=>m.UpdateType == UpdateTypes.Removed)),Times.Once);
            _roleManager.GetRole(role.Id).Should().BeNull();
        }
        
       

    }
}