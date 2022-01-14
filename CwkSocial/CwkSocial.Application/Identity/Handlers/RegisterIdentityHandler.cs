using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using Cwk.Domain.Exceptions;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Options;
using CwkSocial.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CwkSocial.Application.Identity.Handlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSeetings;
    
    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _ctx = ctx;
        _userManager = userManager;
        _jwtSeetings = jwtSettings.Value;
    }
    
    public async Task<OperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

            if (existingIdentity != null)
            {
                result.IsError = true;
                var error = new Error { Code = ErrorCode.IdentityUserAlreadyExists, 
                    Message = $"Provided email address already exists. Cannot register new user"};
                result.Errors.Add(error);
                return result;
            }

            var identity = new IdentityUser
            {
                Email = request.Username,
                UserName = request.Username
            };

            //creating transaction
            using var transaction = _ctx.Database.BeginTransaction();
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
                return result;
            }
            
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            try
            {
                _ctx.UserProfiles.Add(profile);
                await _ctx.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSeetings.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity( new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                    new Claim("IdentityId", identity.Id),
                    new Claim("UserProfileId", profile.UserProfileId.ToString())
                }),
                Expires = DateTime.Now.AddHours(2),
                Audience = _jwtSeetings.Audiences[0],
                Issuer = _jwtSeetings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Payload = tokenHandler.WriteToken(token);
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
}