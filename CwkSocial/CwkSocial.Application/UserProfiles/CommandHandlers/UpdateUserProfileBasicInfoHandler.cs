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
                    .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);

                if (userProfile is null)
                {
                    result.IsError = true;
                    var error = new Error { Code = ErrorCode.NotFound, 
                        Message = $"No UserProfile found with ID {request.UserProfileId}"};
                    result.Errors.Add(error);
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                    request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _ctx.UserProfiles.Update(userProfile);
                await _ctx.SaveChangesAsync();
                
                result.Payload = userProfile;
                return result;
            }
            
            catch (UserProfileNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(e =>
                {
                    var error = new Error { Code = ErrorCode.ValidationError, 
                        Message = $"{ex.Message}"};
                    result.Errors.Add(error);
                });
                
                return result;
            }
            
            catch (Exception e)
            {
                var error = new Error {Code = ErrorCode.ServerError, Message = e.Message };
                result.IsError = true;
                result.Errors.Add(error);
            }
            
            return result;
        }
    }
}
