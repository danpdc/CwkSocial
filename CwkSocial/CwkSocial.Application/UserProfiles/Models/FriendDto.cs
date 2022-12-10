using Cwk.Domain.Aggregates.Friendships;

namespace CwkSocial.Application.UserProfiles.Models;

public class FriendDto
{
    public Guid FriendshipId { get; set; }
    public string? FriendFullName { get; set; }
    public string? City { get; set; }

    public static FriendDto FromFriendship(Friendship friendship, Guid currentUser)
    {
        return new FriendDto
        {
            FriendshipId = friendship.FriendshipId,
            FriendFullName = GetFriendFullName(friendship, currentUser),
            City = GetFriendCity(friendship, currentUser)
        };
    }

    private static string GetFriendFullName(Friendship friendship, Guid currentUser)
    {
        if (friendship.FirstFriendUserProfileId == currentUser)
            return $"{friendship?.SecondFriend?.BasicInfo.FirstName} {friendship?.SecondFriend?.BasicInfo.LastName}";
        
        return $"{friendship?.FirstFriend?.BasicInfo.FirstName} {friendship?.FirstFriend?.BasicInfo.LastName}";
    }

    private static string GetFriendCity(Friendship friendship, Guid currentUser)
    {
        if (friendship.FirstFriendUserProfileId == currentUser)
            return friendship?.SecondFriend?.BasicInfo?.CurrentCity!;
        
        return friendship?.FirstFriend?.BasicInfo?.CurrentCity!;
    }
}