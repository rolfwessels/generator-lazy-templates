using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
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
    public class ProjectValidatorTests
    {

        private ProjectValidator _validator;

        #region Setup/Teardown

        public void Setup()
        {
            _validator = new ProjectValidator();
        }

        [TearDown]
        public void TearDown()
        {

        }

        #endregion

        [Test]
        public void Validate_GiveValidProjectData_ShouldNotFail()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().WithValidData().Build();
            // action
            var validationResult = _validator.Validate(project);
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
            _validator.ShouldHaveValidationErrorFor(project => project.Name, null as string);
        }

        [Test]
        public void Name_GiveLongString_ShouldFail()
        {
            // arrange
            Setup();
            // assert
            _validator.ShouldHaveValidationErrorFor(project => project.Name, GetRandom.String(200));
        }

        
         
    }
}