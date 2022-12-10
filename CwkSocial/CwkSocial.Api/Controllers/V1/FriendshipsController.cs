using CwkSocial.Api.Contracts.Friendships.Requests;
using CwkSocial.Application.Friendships.Commands;

namespace CwkSocial.Api.Controllers.V1;

[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FriendshipsController : BaseController
{
    [HttpPost]
    [Route(ApiRoutes.Friendships.FriendRequestCreate)]
    [ValidateModel]
    public async Task<IActionResult> SendFriendRequest(FriendRequestCreate friendRequestCreate,
        CancellationToken token)
    {
        var command = new CreateFriendRequest
        {
            RequesterId = friendRequestCreate.RequesterId,
            ReceiverId = friendRequestCreate.ReceiverId
        };
        var result = await _mediator.Send(command, token);
        if (result.IsError) HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpPost]
    [Route(ApiRoutes.Friendships.FriendRequestAccept)]
    [ValidateGuid("friendRequestId")]
    public async Task<IActionResult> AcceptFriendRequest(Guid friendRequestId, CancellationToken token)
    {
        var actionPerformedBy = HttpContext.GetUserProfileIdClaimValue();
        var command = new AcceptFriendRequest
        {
            FriendRequestId = friendRequestId,
            ActionPerformedById = actionPerformedBy
        };
        var result = await _mediator.Send(command,token);
        if (result.IsError) return HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpPost]
    [Route(ApiRoutes.Friendships.FriendRequestReject)]
    [ValidateGuid("friendRequestId")]
    public async Task<IActionResult> RejectFriendRequest(Guid friendRequestId, CancellationToken token)
    {
        var actionPerformedBy = HttpContext.GetUserProfileIdClaimValue();
        var command = new RejectFriendRequest
        {
            FriendRequestId = friendRequestId,
            ActionPerformedById = actionPerformedBy
        };
        var result = await _mediator.Send(command,token);
        if (result.IsError) return HandleErrorResponse(result.Errors);
        return NoContent();
    }
}