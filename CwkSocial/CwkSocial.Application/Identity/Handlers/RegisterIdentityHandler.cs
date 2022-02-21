using System.Security.Claims;
using AutoMapper;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using Cwk.Domain.Exceptions;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Identity.Dtos;
using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CwkSocial.Application.Identity.Handlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<IdentityUserProfileDto>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private OperationResult<IdentityUserProfileDto> _result = new();
    private readonly IMapper _mapper;

    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }
    
    public async Task<OperationResult<IdentityUserProfileDto>> Handle(RegisterIdentity request, 
        CancellationToken cancellationToken)
    {
        try
        {
            await ValidateIdentityDoesNotExist(request);
            if (_result.IsError) return _result;
            
            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
            
            var identity = await CreateIdentityUserAsync(request, transaction, cancellationToken);
            if (_result.IsError) return _result;

            var profile = await CreateUserProfileAsync(request, transaction, identity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _result.Payload = _mapper.Map<IdentityUserProfileDto>(profile);
            _result.Payload.UserName = identity.UserName;
            _result.Payload.Token = GetJwtString(identity, profile);
            return _result;
        }
        
        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(ErrorCode.ValidationError, e));
        }
        
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task ValidateIdentityDoesNotExist(RegisterIdentity request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if (existingIdentity != null) 
            _result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.IdentityUserAlreadyExists);
        
    }

    private async Task<IdentityUser> CreateIdentityUserAsync(RegisterIdentity request, 
        IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        var identity = new IdentityUser {Email = request.Username, UserName = request.Username};
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var identityError in createdIdentity.Errors)
            {
                _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
            }
        }
        return identity;
    }

    private async Task<UserProfile> CreateUserProfileAsync(RegisterIdentity request, 
        IDbContextTransaction transaction, IdentityUser identity,
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