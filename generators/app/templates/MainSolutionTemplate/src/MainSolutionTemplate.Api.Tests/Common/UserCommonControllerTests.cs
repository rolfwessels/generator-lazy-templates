using System;
using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.Common
{
    [TestFixture]
    public class UserCommonControllerTests
    {

        private UserCommonController _userCommonController;
        private Mock<ISystemManager> _mockISystemManager;

        #region Setup/Teardown

        public void Setup()
        {
            _mockISystemManager = new Mock<ISystemManager>(MockBehavior.Strict);
            _userCommonController = new UserCommonController(_mockISystemManager.Object);
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
            _userCommonController.Should().NotBeNull();
        }


        [Test]
        public void Get_GivenRequest_ShouldReturnUserReferenceModels()
        {
            // arrange
            Setup();
            var userReference = Builder<UserReference>.CreateListOfSize(2).Build();
            _mockISystemManager.Setup(mc => mc.GetUsersAsReference())
                .Returns(userReference.AsQueryable);
            // action
            var result = _userCommonController.Get().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void GetDetail_GivenRequest_ShouldReturnUserModel()
        {
            // arrange
            Setup();
            var userReference = Builder<User>.CreateListOfSize(2).Build();
            _mockISystemManager.Setup(mc => mc.GetUsers())
                .Returns(userReference.AsQueryable);
            // action
            var result = _userCommonController.GetDetail().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void Get_GivenUserId_ShouldCallGetUser()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.GetUser(user.Id))
                .Returns(user);
            // action
            var result = _userCommonController.Get(user.Id).Result;
            // assert
            result.Id.Should().Be(user.Id);
        }

        [Test]
        public void Put_GivenUserId_ShouldUpdateAGivenUser()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.GetUser(user.Id))
                .Returns(user);
            _mockISystemManager.Setup(mc => mc.SaveUser(user))
                .Returns(user);
            var userDetailModel = new UserDetailModel();
            // action
            var result = _userCommonController.Put(user.Id, userDetailModel).Result;
            // assert
            result.Id.Should().Be(user.Id);
        }

        [Test]
        public void Post_GivenUserId_ShouldAddAUser()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.SaveUser(It.Is<User>(user1 => user1.Email == user.Email.ToLower()))).Returns(user);
            // action
            var result = _userCommonController.Post(user.ToUserModel()).Result;
            // assert
            result.Id.Should().Be(user.Id);
        }


        [Test]
        public void Delete_GivenUserId_ShouldRemoveUser()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.DeleteUser(user.Id)).Returns(user);
            // action
            var result = _userCommonController.Delete(user.Id).Result;
            // assert
            result.Should().Be(true);
        }


        [Test]
        public void Register_GivenRegisterModel_ShouldAddUser()
        {
            // arrange
            Setup();
            var registerModel = Builder<RegisterModel>.CreateNew().Build();
            var user = Builder<User>.CreateNew().Build();
            _mockISystemManager.Setup(mc => mc.SaveUser(It.Is<User>(x => x.Name == registerModel.Name && x.Roles.Any(r=>r.Name == Roles.Guest.Name)))).Returns(user);
            // action
            var result = _userCommonController.Register(registerModel).Result;
            // assert
            result.Name.Should().Be(registerModel.Name);
            
        }

        [Test]
        public void ForgotPassword_GivenEmail_ShouldSendAnEmail()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            // action
            var result = _userCommonController.ForgotPassword(user.Email).Result;
            // assert
            result.Should().BeTrue();

        }


         
    }

}