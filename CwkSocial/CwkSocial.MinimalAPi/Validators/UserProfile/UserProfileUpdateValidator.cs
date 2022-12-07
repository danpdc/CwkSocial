using CwkSocial.MinimalAPi.Contracts.UserProfile.Requests;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Validators.UserProfile;

public class UserProfileUpdateValidator : AbstractValidator<UserProfileCreateUpdate>
{
    public UserProfileUpdateValidator()
    {
        RuleFor(up => up.EmailAddress)
            .NotEmpty()
            .NotNull()
            .EmailAddress();
        RuleFor(up => up.FirstName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);
        RuleFor(up => up.LastName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);
        RuleFor(up => up.DateOfBirth)
            .NotEmpty()
            .NotEqual(default(DateTime));
    }
}