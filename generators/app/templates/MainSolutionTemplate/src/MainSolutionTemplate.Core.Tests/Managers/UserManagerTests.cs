using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Persistance;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class UserManagerTests : BaseTypedManagerTests<User>
    {
        private UserManager _userManager;

        #region Overrides of BaseManagerTests

        public override void Setup()
        {
            base.Setup();
            _userManager = new UserManager(_baseManagerArguments);
        }

        #endregion

        #region Overrides of BaseTypedManagerTests<Project>

        protected override IRepository<User> Repository
        {
            get { return _fakeGeneralUnitOfWork.Users; }
        }

        protected override User SampleObject
        {
            get { return Builder<User>.CreateNew().With(x=>x.Email = GetRandom.Email()).Build(); }
        }

        protected override BaseManager<User> Manager
        {
            get { return _userManager; }
        }

        #endregion
        

        [Test]
        public void SaveUser_WhenCalledWithUser_ShouldToLowerTheEmail()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().With(x=>x.Email = "asdf@GMAIL.com").Build();
            // action
            var result = _userManager.Save(user);
            // assert
            result.Email.Should().Be("asdf@gmail.com");
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
            var userFound = _userManager.GetUserByEmailAndPassword(user.Email, password);
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
            var userFound = _userManager.GetUserByEmailAndPassword(user.Email, password+123);
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
            var userFound = _userManager.GetUserByEmailAndPassword(user.Email+"123", password);
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
            var userFound = _userManager.GetUserByEmail(user.Email);
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
            var userFound = _userManager.GetUserByEmail(user.Email+"123");
            // assert
            userFound.Should().BeNull();
        }


    }
}