using CwkSocial.MinimalAPi.Contracts.Identity;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Validators.Identity;

public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(l => l.Username)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
        RuleFor(l => l.Password)
            .NotNull()
            .NotEmpty();
    }
}