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
using Microsoft.IdentityModel.JsonWebTokens;

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
            await ValidateIdentityDoesNotExist(result, request);
            if (result.IsError) return result;
            
            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
            
            var identity = await CreateIdentityUserAsync(result, request, transaction, cancellationToken);
            if (result.IsError) return result;

            var profile = await CreateUserProfileAsync(result, request, transaction, identity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Payload = GetJwtString(identity, profile);
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

    private async Task ValidateIdentityDoesNotExist(OperationResult<string> result,
        RegisterIdentity request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if (existingIdentity != null) 
            result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.IdentityUserAlreadyExists);
        
    }

    private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result,
        RegisterIdentity request, IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        var identity = new IdentityUser {Email = request.Username, UserName = request.Username};
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var identityError in createdIdentity.Errors)
            {
                result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
            }
        }
        return identity;
    }

    private async Task<UserProfile> CreateUserProfileAsync(OperationResult<string> result,
        RegisterIdentity request, IDbContextTransaction transaction, IdentityUser identity,
        CancellationToken cancellationToken)
    {
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            _ctx.UserProfiles.Add(profile);
            await _ctx.SaveChangesAsync(cancellationToken);
            return profile;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private string GetJwtString(IdentityUser identity, UserProfile profile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, identity.Email),
            new Claim("IdentityId", identity.Id),
            new Claim("UserProfileId", profile.UserProfileId.ToString())
        });

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return _identityService.WriteToken(token);
    }
}