using FluentValidation;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Validation
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1405:ComVisibleTypeBaseTypesShouldBeComVisible")]
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 100);
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}