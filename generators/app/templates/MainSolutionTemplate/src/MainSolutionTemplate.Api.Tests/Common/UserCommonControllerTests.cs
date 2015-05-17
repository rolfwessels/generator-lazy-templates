using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.Common
{
    [TestFixture]
    public class UserCommonControllerTests:BaseCommonControllerTests<User, UserModel, UserReferenceModel, UserDetailModel, IUserManager>
    {
        private Mock<IUserManager> _mockIUserManager;
        private UserCommonController _projectCommonController;


        #region Overrides of BaseCommonControllerTests

        public override void Setup()
        {
            _mockIUserManager = new Mock<IUserManager>(MockBehavior.Strict);
            _projectCommonController = new UserCommonController(_mockIUserManager.Object);
            _mockIUserManager.VerifyAll();
            base.Setup();
        }

        protected override Mock<IUserManager> GetManager()
        {
            return _mockIUserManager;
        }

        protected override BaseCommonController<User, UserModel, UserReferenceModel, UserDetailModel> GetCommonController()
        {
            return _projectCommonController;
        }

        #endregion
        

        [Test]
        public void Register_GivenRegisterModel_ShouldAddUser()
        {
            // arrange
            Setup();
            var registerModel = Builder<RegisterModel>.CreateNew().Build();
            var user = Builder<User>.CreateNew().Build();
            _mockIUserManager.Setup(mc => mc.Save(It.Is<User>(x => x.Name == registerModel.Name && x.Roles.Any(r => r.Name == Roles.Guest.Name)), registerModel.Password)).Returns(user);
            // action
            var result = _projectCommonController.Register(registerModel).Result;
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
            var result = _projectCommonController.ForgotPassword(user.Email).Result;
            // assert
            result.Should().BeTrue();

        }


         
    }

}