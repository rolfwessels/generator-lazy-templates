using FizzWare.NBuilder;
using FluentValidation.TestHelper;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Validation;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Dal.Tests.Validation
{
    [TestFixture]
    public class UserValidatorTests
    {

        private UserValidator _validator;

        #region Setup/Teardown

        public void Setup()
        {
            _validator = new UserValidator();
        }

        [TearDown]
        public void TearDown()
        {

        }

        #endregion

        [Test]
        public void Validate_GiveValidUserData_ShouldNotFail()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().WithValidData().Build();
            // action
            var validationResult = _validator.Validate(user);
            // assert
            validationResult.Errors.Select(x => x.ErrorMessage).StringJoin().Should().BeEmpty();
            validationResult.IsValid.Should().BeTrue();
        }

         
        [Test]
        public void Name_GiveNullName_ShouldFail()
        {
            // arrange
            Setup();
            // assert
            _validator.ShouldHaveValidationErrorFor(user => user.Name, null as string);
        }

             
        [Test]
        public void Email_GiveEmptyEmail_ShouldFail()
        {
            // arrange
            Setup();
            // assert
            _validator.ShouldHaveValidationErrorFor(user => user.Email, null as string);
        }
     
        [Test]
        public void Email_GiveInvalidEmail_ShouldFail()
        {
            // arrange
            Setup();
            // assert
            _validator.ShouldHaveValidationErrorFor(user => user.Email, "test");
        }

        [Test]
        public void Email_GiveValidEmail_ShouldNotFail()
        {
            // arrange
            Setup();
            // assert
            _validator.ShouldNotHaveValidationErrorFor(user => user.Email, "test@test.com");
        }

        [Test]
        public void HashedPassword_GivenEmptyPassword_ShouldFail()
        {
            // arrange
            Setup();
            // assert
            _validator.ShouldHaveValidationErrorFor(user => user.HashedPassword, "");
        }


        [Test]
        public void Validate_GiveInvalidRole_ShouldFail()
        {
            // arrange
            Setup();
            var user = Builder<User>.CreateNew().WithValidData().Build();
            user.Roles.Clear();
            // action
            var validationResult = _validator.Validate(user);
            // assert
            validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("'Roles' should not be empty.");
        }
         
    }
}