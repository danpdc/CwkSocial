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
        public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] PostCommentCreate comment, CancellationToken cancellationToken)
        {
            var isValidGuid = Guid.TryParse(comment.UserProfileId, out var userProfileId);
            if (!isValidGuid)
            {
                var apiError = new ErrorResponse();

                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad Request";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add("Provided User profile id is not in a valid Guid format");

                return BadRequest(apiError);
            }
            
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
    }
}
