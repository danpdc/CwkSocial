using CwkSocial.Application.UserProfiles.CommandHandlers;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;

namespace CwkSocial.Application.UserProfiles.CommandHandlers
{
    internal class DeleteUserProfileHandler : IRequestHandler<DeleteUserProfile, OperationResult<UserProfile>>
    {
        private readonly DataContext _ctx;
        public DeleteUserProfileHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfile request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            var userProfile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);
            
            if (userProfile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            _ctx.UserProfiles.Remove(userProfile);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = userProfile;
            return result;
        }
    }
}
