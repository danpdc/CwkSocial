using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.Friendships.Commands;

public class AcceptFriendRequest : IRequest<OperationResult<Unit>>
{
    public Guid FriendRequestId { get; set; }
    public Guid ActionPerformedById { get; set; }
}

public class AcceptFriendRequestHandler : IRequestHandler<AcceptFriendRequest, OperationResult<Unit>>
{
    private readonly DataContext _ctx;
    private readonly OperationResult<Unit> _result = new();

    public AcceptFriendRequestHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Unit>> Handle(AcceptFriendRequest request, 
        CancellationToken cancellationToken)
    {
        var friendRequest = await _ctx.FriendRequests
            .FirstOrDefaultAsync(fr 
                => fr.FriendRequestId == request.FriendRequestId && 
                   fr.ReceiverUserProfileId == request.ActionPerformedById, cancellationToken);

        if (friendRequest is null)
        {
            _result.AddError(ErrorCode.FriendRequestAcceptNotPossible, 
                "Not possible to accept friend request");
            return _result;
        }

        var friendship = friendRequest.AcceptFriendRequest(Guid.NewGuid());

        await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
        _ctx.FriendRequests.Update(friendRequest);
        _ctx.Friendships.Add(friendship!);

        try
        {
            await _ctx.SaveChangesAsync(cancellationToken);
            await _ctx.Database.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _ctx.Database.RollbackTransactionAsync(cancellationToken);
            _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
        }

        return _result;
    }
}