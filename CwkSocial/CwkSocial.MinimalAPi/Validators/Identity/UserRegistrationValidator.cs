using System.Runtime.InteropServices.JavaScript;
using CwkSocial.MinimalAPi.Contracts.Identity;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Validators.Identity;

public class UserRegistrationValidator : AbstractValidator<UserRegistration>
{
    public UserRegistrationValidator()
    {
        RuleFor(ur => ur.Username)
            .NotNull().WithMessage("Username should not be NULL")
            .NotEmpty().WithMessage("Username should not be empty")
            .EmailAddress().WithMessage("Username must be in email Address format");
        RuleFor(ur => ur.Password).NotNull().NotEmpty();
        RuleFor(ur => ur.FirstName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(10);
        RuleFor(ur => ur.LastName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);
    }
}