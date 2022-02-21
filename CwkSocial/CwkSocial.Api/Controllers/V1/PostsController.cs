using PostInteraction = CwkSocial.Api.Contracts.Posts.Responses.PostInteraction;

namespace CwkSocial.Api.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PostsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllPosts(), cancellationToken);
            var mapped = _mapper.Map<List<PostResponse>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) :  Ok(mapped); 
            
        }
        
        [HttpGet]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var postId = Guid.Parse(id);
            var query = new GetPostById() {PostId = postId};
            var result = await _mediator.Send(query, cancellationToken);
            var mapped = _mapper.Map<PostResponse>(result.Payload);

            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreatePost([FromBody] PostCreate newPost, CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            
            var command = new CreatePost()
            {
                UserProfileId = userProfileId,
                TextContent = newPost.TextContent
            };

            var result = await _mediator.Send(command, cancellationToken);
            var mapped = _mapper.Map<PostResponse>(result.Payload);
            
            return result.IsError ? HandleErrorResponse(result.Errors) 
                : CreatedAtAction(nameof(GetById), new { id = result.Payload.UserProfileId}, mapped);
        }

        [HttpPatch]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePostText([FromBody] PostUpdate updatedPost, string id, CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            
            var command = new UpdatePostText()
            {
                NewText = updatedPost.Text,
                PostId = Guid.Parse(id),
                UserProfileId = userProfileId
            };
            var result = await _mediator.Send(command, cancellationToken);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpDelete]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeletePost(string id, CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var command = new DeletePost() {PostId = Guid.Parse(id), UserProfileId = userProfileId};
            var result = await _mediator.Send(command, cancellationToken);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.PostComments)]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetCommentsByPostId(string postId, CancellationToken cancellationToken)
        {
            var query = new GetPostComments() {PostId = Guid.Parse(postId)};
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsError)  HandleErrorResponse(result.Errors);

            var comments = _mapper.Map<List<PostCommentResponse>>(result.Payload);
            return Ok(comments);
        }

        [HttpPost]
        [Route(ApiRoutes.Posts.PostComments)]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] PostCommentCreate comment, 
            CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();

            var command = new AddPostComment()
            {
                PostId = Guid.Parse(postId),
                UserProfileId = userProfileId,
                CommentText = comment.Text
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            var newComment = _mapper.Map<PostCommentResponse>(result.Payload);

            return Ok(newComment);
        }

        [HttpDelete]
        [Route(ApiRoutes.Posts.CommentById)]
        [ValidateGuid("postId", "commentId")]
        public async Task<IActionResult> RemoveCommentFromPost(string postId, string commentId,
            CancellationToken token)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var postGuid = Guid.Parse(postId);
            var commentGuid = Guid.Parse(commentId);
            var command = new RemoveCommentFromPost
            {
                UserProfileId = userProfileId,
                CommentId = commentGuid,
                PostId = postGuid
            };

            var result = await _mediator.Send(command, token);

            if (result.IsError) return HandleErrorResponse(result.Errors);
            
            return NoContent();
        }

        [HttpPut]
        [Route(ApiRoutes.Posts.CommentById)]
        [ValidateGuid("postId", "commentId")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCommentText(string postId, string commentId,
            PostCommentUpdate updatedComment, CancellationToken token)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var postGuid = Guid.Parse(postId);
            var commentGuid = Guid.Parse(commentId);

            var command = new UpdatePostComment
            {
                UserProfileId = userProfileId,
                PostId = postGuid,
                CommentId = commentGuid,
                UpdatedText = updatedComment.Text
            };

            var result = await _mediator.Send(command, token);

            if (result.IsError) return HandleErrorResponse(result.Errors);
            
            return NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.PostInteractions)]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetPostInteractions(string postId, CancellationToken token)
        {
            var postGuid = Guid.Parse(postId);
            var query = new GetPostInteractions {PostId = postGuid};
            var result = await _mediator.Send(query, token);

            if (result.IsError) HandleErrorResponse(result.Errors);

            var mapped = _mapper.Map<List<PostInteraction>>(result.Payload);
            return Ok(mapped);
        }

        [HttpPost]
        [Route(ApiRoutes.Posts.PostInteractions)]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddPostInteraction(string postId, PostInteractionCreate interaction,
            CancellationToken token)
        {
            var postGuid = Guid.Parse(postId);
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var command = new AddInteraction
            {
                PostId = postGuid,
                UserProfileId = userProfileId,
                Type = interaction.Type
            };

            var result = await _mediator.Send(command, token);

            if (result.IsError) HandleErrorResponse(result.Errors);

            var mapped = _mapper.Map<PostInteraction>(result.Payload);

            return Ok(mapped);
        }

        [HttpDelete]
        [Route(ApiRoutes.Posts.InteractionById)]
        [ValidateGuid("postId", "interactionId")]
        public async Task<IActionResult> RemovePostInteraction(string postId, string interactionId,
            CancellationToken token)
        {
            var postGuid = Guid.Parse(postId);
            var interactionGuid = Guid.Parse(interactionId);
            var userProfileGuid = HttpContext.GetUserProfileIdClaimValue();
            var command = new RemovePostInteraction
            {
                PostId = postGuid,
                InteractionId = interactionGuid,
                UserProfileId = userProfileGuid
            };

            var result = await _mediator.Send(command, token);
            if (result.IsError) return HandleErrorResponse(result.Errors);

            var mapped = _mapper.Map<PostInteraction>(result.Payload);
            
            return Ok(mapped);
        }
    }
}
