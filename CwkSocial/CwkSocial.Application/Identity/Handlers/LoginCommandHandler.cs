using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CwkSocial.Application.Identity.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public LoginCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager, 
        IdentityService identityService)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            var identityUser = await ValidateAndGetIdentityAsync(request, result);
            if (identityUser == null) return result;

            var userProfile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id, cancellationToken: cancellationToken);

            result.Payload = GetJwtString(identityUser, userProfile);
            return result;

        }
        catch (Exception e)
        {
            var error = new Error { Code = ErrorCode.UnknownError, 
                Message = $"{e.Message}"};
            result.IsError = true;
            result.Errors.Add(error);
        }

        return result;
    }

    private async Task<IdentityUser> ValidateAndGetIdentityAsync(LoginCommand request, 
        OperationResult<string> result)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Username);
            
        if (identityUser is null)
        {
            result.IsError = true;
            var error = new Error { Code = ErrorCode.IdentityUserDoesNotExist, 
                Message = $"Unable to find a user with the specified username"};
            result.Errors.Add(error);
            return null;
        }

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!validPassword)
        {
            result.IsError = true;
            var error = new Error { Code = ErrorCode.IncorrectPassword, 
                Message = $"The provided password is incorrect"};
            result.Errors.Add(error);
            return null;
        }

        return identityUser;
    }

    private string GetJwtString(IdentityUser identityUser, UserProfile userProfile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
            new Claim("IdentityId", identityUser.Id),
            new Claim("UserProfileId", userProfile.UserProfileId.ToString())
        });

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return _identityService.WriteToken(token);
    }
}