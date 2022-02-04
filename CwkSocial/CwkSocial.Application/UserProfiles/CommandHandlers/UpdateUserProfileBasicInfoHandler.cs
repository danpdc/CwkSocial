using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cwk.Domain.Exceptions;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;

namespace CwkSocial.Application.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoHandler : IRequestHandler<UpdateUserProfileBasicInfo, OperationResult<UserProfile>>
    {
        private readonly DataContext _ctx;

        public UpdateUserProfileBasicInfoHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfo request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var userProfile = await _ctx.UserProfiles
                    .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);

                if (userProfile is null)
                {
                    result.AddError(ErrorCode.NotFound,
                        string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                    request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _ctx.UserProfiles.Update(userProfile);
                await _ctx.SaveChangesAsync(cancellationToken);
                
                result.Payload = userProfile;
                return result;
            }
            
            catch (UserProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(e => result.AddError(ErrorCode.ValidationError, e));
            }
            
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
            }
            
            return result;
        }
    }
}
