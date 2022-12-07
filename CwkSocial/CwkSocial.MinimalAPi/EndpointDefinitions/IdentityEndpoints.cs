using AutoMapper;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Identity.Queries;
using CwkSocial.MinimalAPi.Abstractions;
using CwkSocial.MinimalAPi.Contracts.Identity;
using CwkSocial.MinimalAPi.Extensions;
using CwkSocial.MinimalAPi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace CwkSocial.MinimalAPi.EndpointDefinitions;

public class IdentityEndpoints : EndpointDefinition
{
    public override void RegisterEndpoints(WebApplication app)
    {
        var identityGroup = app.MapGroup(ApiRoutes.Identity.IdentityBase);

        identityGroup.MapPost(ApiRoutes.Identity.Registration, RegisterUser)
            .AddEndpointFilter<ModelValidationFilter<UserRegistration>>();
        identityGroup.MapPost(ApiRoutes.Identity.Login, UserLogin)
            .AddEndpointFilter<ModelValidationFilter<Login>>();
        identityGroup.MapGet(ApiRoutes.Identity.CurrentUser, GetCurrentUser)
            .RequireAuthorization();
    }

    private async Task<IResult> RegisterUser(IMediator mediator, IMapper mapper,
        UserRegistration registration, CancellationToken token)
    {
        var command = mapper.Map<RegisterIdentity>(registration);
        var result = await mediator.Send(command, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return TypedResults.Ok(mapper.Map<IdentityUserProfile>(result.Payload));
    }

    private async Task<IResult> UserLogin(IMediator mediator, IMapper mapper, 
        Login login, CancellationToken token)
    {
        var command = mapper.Map<LoginCommand>(login);
        var result = await mediator.Send(command, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return TypedResults.Ok(mapper.Map<IdentityUserProfile>(result.Payload));
    }

    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    private async Task<IResult> GetCurrentUser(IMediator mediator, IMapper mapper, 
        HttpContext ctx, CancellationToken token)
    {
        var userProfileId = ctx.GetUserProfileIdClaimValue();

        var query = new GetCurrentUser { UserProfileId = userProfileId, ClaimsPrincipal = ctx.User};
        var result = await mediator.Send(query, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);
        
        return TypedResults.Ok(mapper.Map<IdentityUserProfile>(result.Payload));
    }
}