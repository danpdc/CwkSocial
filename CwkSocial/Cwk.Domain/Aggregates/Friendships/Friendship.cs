using Cwk.Domain.Aggregates.UserProfileAggregate;

namespace Cwk.Domain.Aggregates.Friendships;

public class Friendship
{
    internal Friendship() { }
    public Guid FriendshipId { get; internal set; }
    public Guid? FirstFriendUserProfileId { get; internal set; }
    public UserProfile? FirstFriend { get; internal set; }
    public Guid? SecondFriendUserProfileId { get; internal set; }
    public UserProfile? SecondFriend { get; internal set; }
    public DateTime DateEstablished { get; internal set; }
    public FriendshipStatus FriendshipStatus { get; internal set; }

    public void Unfriend()
    {
        FriendshipStatus = FriendshipStatus.Inactive;
    }
}