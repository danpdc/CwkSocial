using AutoMapper;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.MinimalAPi.Abstractions;
using CwkSocial.MinimalAPi.Contracts.UserProfile.Requests;
using CwkSocial.MinimalAPi.Contracts.UserProfile.Responses;
using CwkSocial.MinimalAPi.Filters;
using MediatR;

namespace CwkSocial.MinimalAPi.EndpointDefinitions;

public class UserProfileEndpoints : EndpointDefinition
{
    public override void RegisterEndpoints(WebApplication app)
    {
        var userProfileGroup = app.MapGroup(ApiRoutes.UserProfiles.UserProfileBase)
            .RequireAuthorization();

        userProfileGroup.MapGet("/", GetAllUserProfiles);
        userProfileGroup.MapGet(ApiRoutes.UserProfiles.IdRoute, GetUserProfileById)
            .AddEndpointFilter<GuidValidationFilter>();
        userProfileGroup.MapPatch(ApiRoutes.UserProfiles.IdRoute, UpdateUserProfile)
            .AddEndpointFilter<GuidValidationFilter>()
            .AddEndpointFilter<ModelValidationFilter<UserProfileCreateUpdate>>();
    }

    private async Task<IResult> GetAllUserProfiles(IMediator mediator, IMapper mapper, CancellationToken token)
    {
        var query = new GetAllUserProfiles();
        var response = await mediator.Send(query, token);
        var profiles = mapper.Map<List<UserProfileResponse>>(response.Payload);
        return TypedResults.Ok(profiles);
    }

    private async Task<IResult> GetUserProfileById(IMediator mediator, IMapper mapper, string id, 
        CancellationToken token)
    {
        var query = new GetUserProfileById { UserProfileId = Guid.Parse(id)};
        var response = await mediator.Send(query, token);

        if (response.IsError)
            return HandleErrorResponse(response.Errors);
            
        var userProfile = mapper.Map<UserProfileResponse>(response.Payload);
        return TypedResults.Ok(userProfile);
    }

    private async Task<IResult> UpdateUserProfile(IMediator mediator, IMapper mapper, 
        string id, UserProfileCreateUpdate updatedProfile, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateUserProfileBasicInfo>(updatedProfile);
        command.UserProfileId = Guid.Parse(id);
        var response = await mediator.Send(command, cancellationToken);

        return response.IsError ? HandleErrorResponse(response.Errors) : Results.NoContent();
    }
}