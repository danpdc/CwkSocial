using AutoMapper;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.Application.Posts.Queries;
using CwkSocial.MinimalAPi.Abstractions;
using CwkSocial.MinimalAPi.Contracts.Post.Requests;
using CwkSocial.MinimalAPi.Contracts.Post.Responses;
using CwkSocial.MinimalAPi.Extensions;
using CwkSocial.MinimalAPi.Filters;
using MediatR;

namespace CwkSocial.MinimalAPi.EndpointDefinitions;

public class PostEndpointDefinition : EndpointDefinition
{
    public override void RegisterEndpoints(WebApplication app)
    {
        var postGroup = app.MapGroup(ApiRoutes.Posts.PostBase)
            .RequireAuthorization();
        postGroup.MapGet("/", GetAllPosts);
        postGroup.MapGet(ApiRoutes.Posts.IdRoute, GetById)
            .WithName(nameof(GetById))
            .AddEndpointFilter<GuidValidationFilter>();
        postGroup.MapPost("/", CreatePost)
            .AddEndpointFilter<ModelValidationFilter<PostCreate>>();
        postGroup.MapPatch(ApiRoutes.Posts.IdRoute, UpdatePostText)
            .AddEndpointFilter<GuidValidationFilter>()
            .AddEndpointFilter<ModelValidationFilter<PostUpdate>>();
        postGroup.MapDelete(ApiRoutes.Posts.IdRoute, DeletePost)
            .AddEndpointFilter<GuidValidationFilter>();
        postGroup.MapGet(ApiRoutes.Posts.PostComments, GetCommentsByPostId)
            .AddEndpointFilter<GuidValidationFilter>();
        postGroup.MapPost(ApiRoutes.Posts.PostComments, AddCommentToPost)
            .AddEndpointFilter<ModelValidationFilter<PostCommentCreate>>();
        postGroup.MapDelete(ApiRoutes.Posts.CommentById, RemoveCommentFromPost);
        postGroup.MapPut(ApiRoutes.Posts.CommentById, UpdateCommentText);
        postGroup.MapGet(ApiRoutes.Posts.PostInteractions, GetPostInteractions);
        postGroup.MapPost(ApiRoutes.Posts.PostInteractions, AddPostInteraction);
        postGroup.MapDelete(ApiRoutes.Posts.InteractionById, RemovePostInteraction);

    }

    private async Task<IResult> RemovePostInteraction(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private async Task<IResult> AddPostInteraction(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private async Task<IResult> GetPostInteractions(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private async Task<IResult> UpdateCommentText(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private async Task<IResult> RemoveCommentFromPost(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private async Task<IResult> AddCommentToPost(HttpContext http, IMediator mediator,
        IMapper mapper, string postId, PostCommentCreate comment, CancellationToken token)
    {
        var userProfileId = http.GetUserProfileIdClaimValue();

        var command = new AddPostComment()
        {
            PostId = Guid.Parse(postId),
            UserProfileId = userProfileId,
            CommentText = comment.Text
        };

        var result = await mediator.Send(command, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var newComment = mapper.Map<PostCommentResponse>(result.Payload);

        return TypedResults.Ok(newComment);
    }

    private async Task<IResult> GetCommentsByPostId(IMediator mediator, IMapper mapper, string postId,
        CancellationToken token)
    {
        var query = new GetPostComments() {PostId = Guid.Parse(postId)};
        var result = await mediator.Send(query, token);

        if (result.IsError)  HandleErrorResponse(result.Errors);

        var comments = mapper.Map<List<PostCommentResponse>>(result.Payload);
        return TypedResults.Ok(comments);
    }

    private async Task<IResult> DeletePost(IMediator mediator, string id, CancellationToken token,
        HttpContext http)
    {
        var userProfileId = http.GetUserProfileIdClaimValue();
        var command = new DeletePost() {PostId = Guid.Parse(id), UserProfileId = userProfileId};
        var result = await mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : Results.NoContent();
    }

    private async Task<IResult> UpdatePostText(HttpContext http, IMediator mediator,
        PostUpdate updatedPost, string id, CancellationToken token)
    {
        var userProfileId = http.GetUserProfileIdClaimValue();
            
        var command = new UpdatePostText()
        {
            NewText = updatedPost.Text,
            PostId = Guid.Parse(id),
            UserProfileId = userProfileId
        };
        var result = await mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : Results.NoContent();
    }

    private async Task<IResult> CreatePost(IMediator mediator, IMapper mapper,
        PostCreate newPost, HttpContext http, CancellationToken token)
    {
        var userProfileId = http.GetUserProfileIdClaimValue();
            
        var command = new CreatePost()
        {
            UserProfileId = userProfileId,
            TextContent = newPost.TextContent
        };

        var result = await mediator.Send(command, token);
        var mapped = mapper.Map<PostResponse>(result.Payload);
            
        return result.IsError ? HandleErrorResponse(result.Errors) 
            : TypedResults.CreatedAtRoute(mapped, nameof(GetById), new {id = mapped.PostId});
    }

    private async Task<IResult> GetById(IMediator mediator, IMapper mapper,
        string id, CancellationToken token)
    {
        var postId = Guid.Parse(id);
        var query = new GetPostById() {PostId = postId};
        var result = await mediator.Send(query, token);
        var mapped = mapper.Map<PostResponse>(result.Payload);
        return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(mapped);
    }

    private async Task<IResult> GetAllPosts(IMediator mediator, IMapper mapper, CancellationToken token)
    {
        var result = await mediator.Send(new GetAllPosts(), token);
        var mapped = mapper.Map<List<PostResponse>>(result.Payload);
        return result.IsError ? HandleErrorResponse(result.Errors) :  TypedResults.Ok(mapped); 
    }
}