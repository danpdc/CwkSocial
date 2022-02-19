using CwkSocial.Api.Services.JWTService;

namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class IdentityController : BaseController
{

    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register(UserRegistration registration, CancellationToken cancellationToken, [FromServices] IJWTService jwtService)
    {
        var command = Mapper.Map<RegisterIdentity>(registration);
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var jwt = jwtService.GetJwtString(result.Payload.IdentityUser, result.Payload.UserProfile);
        var authenticationResult = new AuthenticationResult(jwt);
        
        return Ok(authenticationResult);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(Login login, CancellationToken cancellationToken, [FromServices] IJWTService jwtService)
    {
        var command = Mapper.Map<LoginCommand>(login);
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var jwt=jwtService.GetJwtString(result.Payload.IdentityUser,result.Payload.UserProfile);
        var authenticationResult = new AuthenticationResult(jwt);
        
        return Ok(authenticationResult);
    }

    [HttpDelete]
    [Route(ApiRoutes.Identity.IdentityById)]
    [ValidateGuid("identityUserId")]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteAccount(string identityUserId, CancellationToken token)
    {
        var identityUserGuid = Guid.Parse(identityUserId);
        var requestorGuid = HttpContext.GetIdentityIdClaimValue();
        var command = new RemoveAccount
        {
            IdentityUserId = identityUserGuid,
            RequestorGuid = requestorGuid
        };
        var result = await Mediator.Send(command, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);
        
        return NoContent();
    }
}