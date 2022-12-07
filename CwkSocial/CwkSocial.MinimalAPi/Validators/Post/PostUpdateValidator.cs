using CwkSocial.MinimalAPi.Contracts.Post.Requests;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Validators.Post;

public class PostUpdateValidator : AbstractValidator<PostUpdate>
{
    public PostUpdateValidator()
    {
        RuleFor(pu => pu.Text)
            .NotNull()
            .NotEmpty();
    }
}