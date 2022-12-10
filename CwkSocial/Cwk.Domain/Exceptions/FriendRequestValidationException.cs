namespace Cwk.Domain.Exceptions;

public class FriendRequestValidationException : DomainModelInvalidException
{
    internal FriendRequestValidationException() {}
    internal FriendRequestValidationException(string message) : base(message) {}
    internal FriendRequestValidationException(string message, Exception inner) : base(message, inner) {}
}