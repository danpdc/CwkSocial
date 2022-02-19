using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Models.Identity;
using CwkSocial.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CwkSocial.Application.Identity.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<LoginResponse>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;

    public LoginCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<OperationResult<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<LoginResponse>();
        try
        {
            var identityUser = await getIdentityAsync(request, result);
            if (result.IsError) return result;

            var userProfile = await getUserProfile(result, identityUser, cancellationToken);
            if (result.IsError) return result;

            result.Payload = new LoginResponse(identityUser, userProfile);
            return result;

        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }

    private async Task<UserProfile> getUserProfile(OperationResult<LoginResponse> result, IdentityUser identityUser, CancellationToken cancellationToken)
    {
        var userProfile = await _ctx.UserProfiles
                        .FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id, cancellationToken);

        if (userProfile == null)
        {
            result.AddError(ErrorCode.UserProfileNotFound, IdentityErrorMessages.NonExistentProfileUser);
        }

        return userProfile;
    }

    private async Task<IdentityUser> getIdentityAsync(LoginCommand request,
        OperationResult<LoginResponse> result)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Username);

        if (identityUser is null)
        {
            result.AddError(ErrorCode.IdentityUserDoesNotExist, IdentityErrorMessages.NonExistentIdentityUser);
            return null;
        }

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!validPassword)
            result.AddError(ErrorCode.IncorrectPassword, IdentityErrorMessages.IncorrectPassword);

        return identityUser;
    }


}