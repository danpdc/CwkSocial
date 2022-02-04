using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;

namespace CwkSocial.Application.UserProfiles.QueryHandlers
{
    internal class GetUserProfileByIdHandler 
        : IRequestHandler<GetUserProfileById, OperationResult<UserProfile>>
    {
        private readonly DataContext _ctx;

        public GetUserProfileByIdHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task<OperationResult<UserProfile>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            
            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);
            
            if (profile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            result.Payload = profile;
            return result;
        }
    }
}
