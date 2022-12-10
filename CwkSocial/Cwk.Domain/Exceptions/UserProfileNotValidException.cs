namespace Cwk.Domain.Exceptions;

public class UserProfileNotValidException : DomainModelInvalidException
{
    internal UserProfileNotValidException() {}
    internal UserProfileNotValidException(string message) : base(message) {}
    internal UserProfileNotValidException(string message, Exception inner) : base(message, inner) {}
}