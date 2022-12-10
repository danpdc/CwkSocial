using Cwk.Domain.Aggregates.Friendships;
using Cwk.Domain.Exceptions;
using FluentValidation.Results;

namespace Cwk.Domain.Validators.FriendshipsValidators;

public class FriendshipAggregateValidator
{
    /// <summary>
    /// Validates a friend request instance
    /// </summary>
    /// <param name="friendRequest">The friend request instance to be validated</param>
    /// <exception cref="FriendRequestValidationException">Thrown when the instance is not valid</exception>
    public static void ValidateFriendRequest(FriendRequest friendRequest)
    {
        var validator = new FriendRequestValidator();
        var validationResult = validator.Validate(friendRequest);

        if (!validationResult.IsValid) 
            ThrowNotValidException<FriendRequestValidationException>(validationResult.Errors);
    }

    private static void ThrowNotValidException<T>(List<ValidationFailure> errors) 
        where T : DomainModelInvalidException
    {
        var exception = new FriendRequestValidationException("Friend request is not  valid");
        errors
            .ForEach(e => exception.ValidationErrors.Add(e.ErrorMessage));
        throw exception;
    }
}