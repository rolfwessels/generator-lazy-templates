using FluentValidation;
using FluentValidation.Results;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Validation
{
    public interface IValidatorFactory
    {
        ValidationResult For<T>(T user);
        void ValidateAndThrow<T>(T user);
        IValidator<T> Validator<T>();
    }

    
}