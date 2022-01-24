using Cwk.Domain.Aggregates.PostAggregate;
using Cwk.Domain.Exceptions;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.Dal;
using MediatR;

namespace CwkSocial.Application.Posts.CommandHandlers;

public class CreatePostHandler : IRequestHandler<CreatePost, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public CreatePostHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Post>> Handle(CreatePost request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = Post.CreatePost(request.UserProfileId, request.TextContent);
            _ctx.Posts.Add(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (PostNotValidException e)
        {
            result.IsError = true;
            e.ValidationErrors.ForEach(er =>
            {
                var error = new Error
                {
                    Code = ErrorCode.ValidationError,
                    Message = $"{e.Message}"
                };
                result.Errors.Add(error);
            });
        }

        catch (Exception e)
        {
            var error = new Error { Code = ErrorCode.UnknownError, 
                Message = $"{e.Message}"};
            result.IsError = true;
            result.Errors.Add(error);
        }
        
        return result;
    }
}