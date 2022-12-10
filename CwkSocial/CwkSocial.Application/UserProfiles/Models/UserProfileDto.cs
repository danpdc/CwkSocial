using Cwk.Domain.Aggregates.Friendships;
using Cwk.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Application.UserProfiles.Models;

public class UserProfileDto
{
    public Guid UserProfileId { get; set; }
    public UserInfoDto? UserInfo { get; set; }
    public List<FriendRequestDto> FriendRequests { get; set; } = new();
    public List<FriendDto> Friends { get; set; } = new();

    public static UserProfileDto FromUserProfile(UserProfile profile,
        List<FriendRequest> friendRequests, List<Friendship> friendships)
    {
        var userProfileDto = new UserProfileDto { UserProfileId = profile.UserProfileId};
        userProfileDto.UserInfo = UserInfoDto.FromBasicInfo(profile.BasicInfo);
        friendRequests.ForEach(fr 
            => userProfileDto.FriendRequests.Add(FriendRequestDto.FromFriendRequest(fr)));
        friendships.ForEach(f 
            => userProfileDto.Friends.Add(FriendDto.FromFriendship(f, userProfileDto.UserProfileId)));

        return userProfileDto;
    }
}