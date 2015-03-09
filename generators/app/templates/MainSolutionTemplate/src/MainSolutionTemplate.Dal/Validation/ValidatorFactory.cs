using System;
using Autofac;
using FluentValidation;
using FluentValidation.Results;

namespace MainSolutionTemplate.Dal.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly Func<IComponentContext> _componentContext;


        public ValidatorFactory(Func<IComponentContext> componentContext)
        {
            _componentContext = componentContext;
        }


        public ValidationResult For<T>(T user)
        {
            var validationRules = GetValidationRules<T>();
            return validationRules != null ? validationRules.Validate(user) : new ValidationResult();
        }

        public void ValidateAndThrow<T>(T user)
        {
            var validationRules = GetValidationRules<T>();
            if (validationRules != null) validationRules.ValidateAndThrow(user);
        }

        public IValidator<T> Validator<T>()
        {
            return GetValidationRules<T>();
        }

        protected IValidator<T> GetValidationRules<T>()
        {
            IValidator<T> output;
            _componentContext().TryResolve(out output);
            return output;
        }
    }
}