using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class UserManagerTests : BaseTypedManagerTests<User>
    {
        private UserManager _userManager;

        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            _userManager = new UserManager(_baseManagerArguments);
        }

        #endregion

        [Test]
        public void GetUserByEmailAndPassword_WhenCalledWithExistingUsernameAndPassword_ShouldReturnTheUser()
        {
            // arrange
            Setup();
            User user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            const string password = "sample";
            user.HashedPassword = PasswordHash.CreateHash(password);
            // action
            User userFound = _userManager.GetUserByEmailAndPassword(user.Email, password).Result;
            // assert
            userFound.Should().NotBeNull();
        }

        [Test]
        public void GetUserByEmailAndPassword_WhenCalledWithExistingUsernameWithInvalidPassword_ShouldReturnNull()
        {
            // arrange
            Setup();
            User user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            const string password = "sample";
            user.HashedPassword = PasswordHash.CreateHash(password);
            // action
            User userFound = _userManager.GetUserByEmailAndPassword(user.Email, password + 123).Result;
            // assert
            userFound.Should().BeNull();
        }

        [Test]
        public void GetUserByEmailAndPassword_WhenCalledWithInvalidUser_ShouldReturnNull()
        {
            // arrange
            Setup();
            User user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            const string password = "sample";
            user.HashedPassword = PasswordHash.CreateHash(password);
            // action
            User userFound = _userManager.GetUserByEmailAndPassword(user.Email + "123", password).Result;
            // assert
            userFound.Should().BeNull();
        }


        [Test]
        public void GetUserByEmail_WhenCalledWithExistingUserWithInvalidEmail_ShouldReturnNull()
        {
            // arrange
            Setup();
            User user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            // action
            User userFound = _userManager.GetUserByEmail(user.Email + "123").Result;
            // assert
            userFound.Should().BeNull();
        }

        [Test]
        public void GetUserByEmail_WhenCalledWithExistingUserWithInvalidPassword_ShouldReturnUser()
        {
            // arrange
            Setup();
            User user = _fakeGeneralUnitOfWork.Users.AddFake().First();
            // action
            User userFound = _userManager.GetUserByEmail(user.Email).Result;
            // assert
            userFound.Should().NotBeNull();
        }

        [Test]
        public void SaveUser_WhenCalledWithUser_ShouldToLowerTheEmail()
        {
            // arrange
            Setup();
            User user = Builder<User>.CreateNew().With(x => x.Email = "asdf@GMAIL.com").Build();
            // action
            User result = _userManager.Save(user).Result;
            // assert
            result.Email.Should().Be("asdf@gmail.com");
        }

        protected override IRepository<User> Repository
        {
            get { return _fakeGeneralUnitOfWork.Users; }
        }

        protected override User SampleObject
        {
            get { return Builder<User>.CreateNew().With(x => x.Email = GetRandom.Email()).Build(); }
        }

        protected override BaseManager<User> Manager
        {
            get { return _userManager; }
        }
    }
}