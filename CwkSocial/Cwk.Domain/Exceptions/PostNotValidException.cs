namespace Cwk.Domain.Exceptions;

public class PostNotValidException : NotValidException
{
    internal PostNotValidException() {}
    internal PostNotValidException(string message) : base(message) {}
    internal PostNotValidException(string message, Exception inner) : base(message, inner) {}
}