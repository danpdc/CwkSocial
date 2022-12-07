using CwkSocial.MinimalAPi.Contracts.Post.Requests;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Validators.Post;

public class PostCreateValidator : AbstractValidator<PostCreate>
{
    public PostCreateValidator()
    {
        RuleFor(pc => pc.TextContent)
            .NotEmpty()
            .NotNull();
    }
}