using System.Security.Claims;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using Cwk.Domain.Exceptions;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CwkSocial.Application.Identity.Handlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    
    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    public async Task<OperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            var creationValidated = await ValidateIdentityDoesNotExist(result, request);
            if (!creationValidated) return result;
            
            await using var transaction = _ctx.Database.BeginTransaction();
            
            var identity = await CreateIdentityUserAsync(result, request, transaction);
            if (identity == null) return result;

            var profile = await CreateUserProfileAsync(result, request, transaction, identity);
            await transaction.CommitAsync();

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                new Claim("IdentityId", identity.Id),
                new Claim("UserProfileId", profile.UserProfileId.ToString())
            });

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            result.Payload = _identityService.WriteToken(token);
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

    private async Task<bool> ValidateIdentityDoesNotExist(OperationResult<string> result,
        RegisterIdentity request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if (existingIdentity != null)
        {
            result.IsError = true;
            var error = new Error { Code = ErrorCode.IdentityUserAlreadyExists, 
                Message = $"Provided email address already exists. Cannot register new user"};
            result.Errors.Add(error);
            return false;
        }

        return true;
    }

    private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result,
        RegisterIdentity request, IDbContextTransaction transaction)
    {
        var identity = new IdentityUser {Email = request.Username, UserName = request.Username};
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync();
            result.IsError = true;

            foreach (var identityError in createdIdentity.Errors)
            {
                var error = new Error { Code = ErrorCode.IdentityCreationFailed, 
                    Message = identityError.Description};
                result.Errors.Add(error);
            }
            return null;
        }

        return identity;
    }

    private async Task<UserProfile> CreateUserProfileAsync(OperationResult<string> result,
        RegisterIdentity request, IDbContextTransaction transaction, IdentityUser identity)
    {
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            _ctx.UserProfiles.Add(profile);
            await _ctx.SaveChangesAsync();
            return profile;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}