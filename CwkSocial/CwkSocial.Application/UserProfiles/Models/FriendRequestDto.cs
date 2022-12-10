using Cwk.Domain.Aggregates.Friendships;
using MediatR.Wrappers;

namespace CwkSocial.Application.UserProfiles.Models;

public class FriendRequestDto
{
    public Guid FriendRequestId { get; set; }
    public string? RequesterFullname { get; set; }
    public string? City { get; set; }

    public static FriendRequestDto FromFriendRequest(FriendRequest request)
    {
        return new FriendRequestDto
        {
            FriendRequestId = request.FriendRequestId,
            RequesterFullname = $"{request.Requester.BasicInfo.FirstName} {request.Requester.BasicInfo.LastName}",
            City = request.Requester.BasicInfo.CurrentCity
        };
    }
}