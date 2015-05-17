using System;
using FizzWare.NBuilder;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.Tests.Managers;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Core.Tests
{
	[TestFixture]
	public class BaseManagerUserManagerTests : BaseManagerTests
	{
	    private UserManager _userManager;

	    #region Overrides of BaseManagerTests

        public override void Setup()
        {
            base.Setup();
            _userManager = new UserManager(_baseManagerArguments);
        }

        #endregion
        

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
            _userManager.Should().NotBeNull();
		}

	    [Test]
        public void GetUsers_WhenCalled_ShouldReturnUsers()
	    {
	        // arrange
	        Setup();
	        var user = Builder<User>.CreateNew().Build();
	        _fakeGeneralUnitOfWork.Users.Add(user);
	        // action
	        var users = _userManager.Get();
	        // assert
	        users.Should().Contain(user);
	    }
  
        [Test]
        public void GetUser_WhenCalled_ShouldReturnUsers()
	    {
	        // arrange
	        Setup();
	        var userAdd = Builder<User>.CreateNew().Build();
	        _fakeGeneralUnitOfWork.Users.Add(userAdd);
	        // action
            var user = _userManager.Get(userAdd.Id);
	        // assert
            user.Id.Should().Be(userAdd.Id);
	    }


        [Test]
        public void SaveUser_WhenCalled_ShouldAddUsers()
	    {
	        // arrange
	        Setup();
	        var userAdd = Builder<User>.CreateNew().Build();
	        // action
            var user = _userManager.Save(userAdd);
	        // assert
            _fakeGeneralUnitOfWork.Users.Should().Contain(user);
	    }


	    [Test]
	    public void SaveUser_WhenCalledWithInvalidData_ShouldNotAddUsersAndThrowException()
	    {
	        // arrange
	        Setup();
	        var userAdd = Builder<User>.CreateNew().Build();
	        _mockIValidatorFactory.Setup(mc => mc.ValidateAndThrow(userAdd))
                                  .Throws(new Exception("Where is the Name"));
	        // action
	        Action testCall = () => { _userManager.Save(userAdd); };
	        // assert
	        testCall.ShouldThrow<Exception>().WithMessage("Where is the Name");
	        _fakeGeneralUnitOfWork.Users.Should().NotContain(userAdd);
	    }

	    [Test]
        public void DeleteUser_WhenCalledWithExistingItem_ShouldRemoveThatUser()
	    {
	        // arrange
	        Setup();
            var userAdd = Builder<User>.CreateNew().Build();
            _fakeGeneralUnitOfWork.Users.Add(userAdd);
	        // action
	        var deleteUser = _userManager.Delete(userAdd.Id);
	        // assert
	        deleteUser.Should().NotBeNull();
	    }
        
        [Test]
        public void DeleteUser_WhenCalledNonWithExistingItem_ShouldRemoveThatUser()
	    {
	        // arrange
	        Setup();
            var userAdd = Builder<User>.CreateNew().Build();
	        // action
	        var deleteUser = _userManager.Delete(userAdd.Id);
	        // assert
	        deleteUser.Should().BeNull();
	    }


	    [Test]
        public void GetUserByEmail_WhenCalledWithExistingItem_ShouldReturnThatUser()
	    {
	        // arrange
	        Setup();
            var userAdd = Builder<User>.CreateNew().Build();
            _userManager.Save(userAdd);
	        // action
	        var deleteUser = _userManager.GetUserByEmail(userAdd.Email);
	        // assert
	        deleteUser.Should().NotBeNull();
	    }
        
        [Test]
        public void GetUserByEmail_WhenCalledNonWithExistingItem_ShouldReturnNull()
	    {
	        // arrange
	        Setup();
            var userAdd = Builder<User>.CreateNew().Build();
	        // action
            var deleteUser = _userManager.GetUserByEmail(userAdd.Email);
	        // assert
	        deleteUser.Should().BeNull();
	    }

	    [Test]
        public void GetUserByEmailAndPassword_WhenCalledNonWithExistingItem_ShouldReturnNewUser()
	    {
	        // arrange
	        Setup();
            var userAdd = Builder<User>.CreateNew().Build();
	        // action
            var user = _userManager.GetUserByEmailAndPassword(userAdd.Email , "Password");
	        // assert
	        user.Should().BeNull();
	    }

	    [Test] 
	    public void GetUserByEmailAndPassword_WhenCalledWithExistingItem_ShouldReturnNull()
	    {
	        // arrange
	        Setup();
	        var userAdd = Builder<User>.CreateNew().Build();
            _userManager.Save(userAdd, "Password");
	        // action
            var user = _userManager.GetUserByEmailAndPassword(userAdd.Email, "Password");
	        // assert
	        user.Should().NotBeNull();
	    }
	}
}