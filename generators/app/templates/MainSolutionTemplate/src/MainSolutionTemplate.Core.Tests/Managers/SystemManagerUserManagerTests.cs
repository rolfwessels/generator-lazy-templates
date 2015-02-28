using System;
using System.Linq;
using AutoMapper;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Utilities.Helpers;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class SystemManagerUserManagerTests : SystemManagerTests
    {
        
        [Test]
        public void GetUsers_WhenCalled_ShouldReturnUsers()
        {
            // arrange
            Setup();
            const int expected = 2;
            _fakeGeneralUnitOfWork.Users.AddFake(expected);
            // action
            var result = _systemManager.GetUsers();
            // assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public void GetUser_WhenCalledWithId_ShouldReturnSingleUser()
        {
            // arrange
            Setup();
            var addFake = _fakeGeneralUnitOfWork.Users.AddFake();
            var guid = addFake.First().Id;
            // action
            var result = _systemManager.GetUser(guid);
            // assert
            result.Id.Should().Be(guid);
        }

        [Test]
        public void SaveUser_WhenCalledWithUser_ShouldSaveTheUser()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            // action
            var result = _systemManager.SaveUser(user);
            // assert
            _fakeGeneralUnitOfWork.Users.Should().HaveCount(1);

        }

        [Test]
        public void SaveUser_WhenCalledWithUser_ShouldToLowerTheEmail()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            // action
            var result = _systemManager.SaveUser(user);
            // assert
            result.Email.Should().Be(user.Email);
        }

        [Test]
        public void SaveUser_WhenCalledWithUser_ShouldCallMessageThatDataWasInserted()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().Build();
            // action
            _systemManager.SaveUser(user);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<User>>(m=>m.UpdateType == UpdateTypes.Inserted)),Times.Once);
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<User>>(m => m.UpdateType == UpdateTypes.Updated)), Times.Never);
        }


        [Test]
        public void SaveUser_WhenCalledWithExistingUser_ShouldCallMessageThatDataWasUpdated()
        {
            // arrange
            Setup();
            var user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            // action
            _systemManager.SaveUser(user);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<User>>(m=>m.UpdateType == UpdateTypes.Updated)),Times.Once);
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<User>>(m => m.UpdateType == UpdateTypes.Inserted)), Times.Never);
        }

        [Test]
        public void DeleteUser_WhenCalledWithExistingUser_ShouldCallMessageThatDataWasRemoved()
        {
            // arrange
            Setup();
            var user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            // action
            _systemManager.DeleteUser(user.Id);
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<User>>(m=>m.UpdateType == UpdateTypes.Removed)),Times.Once);
            _systemManager.GetUser(user.Id).Should().BeNull();
        }
        
        
        [Test]
        public void GetUserByEmailAndPassword_WhenCalledWithExistingUsernameAndPassword_ShouldReturnTheUser()
        {
            // arrange
            Setup();
            var user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            const string password = "sample";
            user.HashedPassword = PasswordHash.CreateHash(password);
            // action
            var userFound = _systemManager.GetUserByEmailAndPassword(user.Email, password);
            // assert
            userFound.Should().NotBeNull();
        }
  
        [Test]
        public void GetUserByEmailAndPassword_WhenCalledWithExistingUsernameWithInvalidPassword_ShouldReturnNull()
        {
            // arrange
            Setup();    
            var user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            const string password = "sample";
            user.HashedPassword = PasswordHash.CreateHash(password);
            // action
            var userFound = _systemManager.GetUserByEmailAndPassword(user.Email, password+123);
            // assert
            userFound.Should().BeNull();
        }
        
        [Test]
        public void GetUserByEmailAndPassword_WhenCalledWithInvalidUser_ShouldReturnNull()
        {
            // arrange
            Setup();    
            var user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            const string password = "sample";
            user.HashedPassword = PasswordHash.CreateHash(password);
            // action
            var userFound = _systemManager.GetUserByEmailAndPassword(user.Email+"123", password);
            // assert
            userFound.Should().BeNull();
        }
    
        [Test]
        public void GetUserByEmail_WhenCalledWithExistingUserWithInvalidPassword_ShouldReturnUser()
        {
            // arrange
            Setup();    
            var user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            // action
            var userFound = _systemManager.GetUserByEmail(user.Email);
            // assert
            userFound.Should().NotBeNull();
        }
    
        
        [Test]
        public void GetUserByEmail_WhenCalledWithExistingUserWithInvalidEmail_ShouldReturnNull()
        {
            // arrange
            Setup();    
            var user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            // action
            var userFound = _systemManager.GetUserByEmail(user.Email+"123");
            // assert
            userFound.Should().BeNull();
        }


    }
}