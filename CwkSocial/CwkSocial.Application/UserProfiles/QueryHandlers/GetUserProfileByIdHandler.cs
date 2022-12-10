using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Models;

namespace CwkSocial.Application.UserProfiles.QueryHandlers
{
    internal class GetUserProfileByIdHandler 
        : IRequestHandler<GetUserProfileById, OperationResult<UserProfileDto>>
    {
        private readonly DataContext _ctx;

        public GetUserProfileByIdHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task<OperationResult<UserProfileDto>> Handle(GetUserProfileById request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfileDto>();
            
            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, 
                    cancellationToken: cancellationToken);
            
            if (profile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            var friendRequests = await _ctx.FriendRequests
                .Where(fr => fr.ReceiverUserProfileId == request.UserProfileId)
                .ToListAsync();

            var friendships = await _ctx.Friendships
                .Where(f => f.FirstFriendUserProfileId == request.UserProfileId
                            || f.SecondFriendUserProfileId == request.UserProfileId)
                .ToListAsync();
            
            result.Payload = UserProfileDto.FromUserProfile(profile, friendRequests, friendships);
            return result;
        }
    }
}
