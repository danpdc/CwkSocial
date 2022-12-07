using CwkSocial.MinimalAPi.Contracts.Post.Requests;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Validators.Post;

public class PostCommentCreateValidator : AbstractValidator<PostCommentCreate>
{
    public PostCommentCreateValidator()
    {
        RuleFor(pcc => pcc.Text)
            .NotEmpty()
            .NotNull();
    }
}