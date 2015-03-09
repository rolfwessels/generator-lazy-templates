using FizzWare.NBuilder;
using FluentValidation.TestHelper;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Tests.Helpers;
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
        public void Validate_GiveNullName_ShouldFail()
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

         
    }
}