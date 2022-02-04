using Cwk.Domain.Aggregates.PostAggregate;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.Posts.CommandHandlers;

public class DeletePostHandler : IRequestHandler<DeletePost, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public DeletePostHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Post>> Handle(DeletePost request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);
            
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound, 
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                
                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.PostDeleteNotPossible, PostsErrorMessages.PostDeleteNotPossible);
                return result;
            }

            _ctx.Posts.Remove(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}