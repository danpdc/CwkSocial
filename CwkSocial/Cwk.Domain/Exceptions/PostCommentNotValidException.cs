using System.Drawing;

namespace Cwk.Domain.Exceptions;

public class PostCommentNotValidException : NotValidException
{
    internal PostCommentNotValidException() {}
    internal PostCommentNotValidException(string message) : base(message) {}
    internal PostCommentNotValidException(string message, Exception inner) : base(message, inner) {}
}