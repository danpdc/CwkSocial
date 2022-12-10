using Cwk.Domain.Aggregates.UserProfileAggregate;
using Cwk.Domain.Exceptions;
using Cwk.Domain.Validators.FriendshipsValidators;

namespace Cwk.Domain.Aggregates.Friendships;

public class FriendRequest
{
    private FriendRequest() {}
    public Guid FriendRequestId { get; private set; }
    public Guid RequesterUserProfileId { get; private set; }
    public UserProfile? Requester { get; private set; }
    public Guid ReceiverUserProfileId { get; private set; }
    public UserProfile? Receiver { get; private set; }
    public DateTime DateSent { get; private set; }
    public DateTime DateResponded { get; private set; }
    public ResponseType Response { get; private set; }

    /// <summary>
    /// Create new instance of a friend request
    /// </summary>
    /// <param name="friendRequestId">The friend request identifier</param>
    /// <param name="requesterId">User profile identifier of the requester</param>
    /// <param name="receiverId">User profile identifier of the receiver</param>
    /// <param name="dateSent">The date when the request was initiated</param>
    /// <returns><see cref="FriendRequest"/></returns>
    /// <exception cref="FriendRequestValidationException"></exception>
    public static FriendRequest CreateFriendRequest(Guid friendRequestId, Guid requesterId, Guid receiverId,
        DateTime dateSent)
    {
        var friendRequest = new FriendRequest();
        friendRequest.FriendRequestId = friendRequestId;
        friendRequest.RequesterUserProfileId = requesterId;
        friendRequest.ReceiverUserProfileId = receiverId;
        friendRequest.DateSent = dateSent;
        friendRequest.Response = ResponseType.Pending;

        FriendshipAggregateValidator.ValidateFriendRequest(friendRequest);
        
        return friendRequest;
    }
    
    #region Behavior

    public Friendship? AcceptFriendRequest(Guid friendshipId)
    {
        var friendship = new Friendship
        {
            FriendshipId = friendshipId,
            FirstFriendUserProfileId = RequesterUserProfileId,
            SecondFriendUserProfileId = ReceiverUserProfileId,
            DateEstablished = DateTime.UtcNow,
            FriendshipStatus = FriendshipStatus.Active
        };

        Response = ResponseType.Accepted;
        DateResponded = DateTime.UtcNow;
        return friendship;
    }

    public void RejectFriendRequest()
    {
        Response = ResponseType.Declined;
        DateResponded = DateTime.UtcNow;
    }
    #endregion
    
}