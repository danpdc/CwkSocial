using Cwk.Domain.Aggregates.Friendships;
using Cwk.Domain.Exceptions;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Dal;
using MediatR;

namespace CwkSocial.Application.Friendships.Commands;

public class CreateFriendRequest : IRequest<OperationResult<Unit>>
{
    public Guid RequesterId { get; set; }
    public Guid ReceiverId { get; set; }
}

public class CreateFriendRequestHandler : IRequestHandler<CreateFriendRequest, OperationResult<Unit>>
{
    private readonly DataContext _ctx;
    private readonly OperationResult<Unit> _result = new();

    public CreateFriendRequestHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<Unit>> Handle(CreateFriendRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var friendRequest = FriendRequest
                .CreateFriendRequest(Guid.NewGuid(), request.RequesterId, request.ReceiverId, DateTime.UtcNow);
            _ctx.FriendRequests.Add(friendRequest);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (FriendRequestValidationException ex)
        {
            _result.AddError(ErrorCode.FriendRequestValidationError, ex.Message);
        }
        catch (Exception e)
        {
            _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
        }
        
        return _result;
    }
}