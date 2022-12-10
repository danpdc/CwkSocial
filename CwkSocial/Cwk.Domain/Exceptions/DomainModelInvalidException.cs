namespace Cwk.Domain.Exceptions;

// TO DO: Think about a different naming for this exception that pinpoints more precisely what the purpose
// of the exception is. Now it's broad. 
public abstract class DomainModelInvalidException : Exception
{
    internal DomainModelInvalidException()
    {
        ValidationErrors = new List<string>();
    }
    
    internal DomainModelInvalidException(string message) : base(message)
    {
        ValidationErrors = new List<string>();
    }
    
    internal DomainModelInvalidException(string message, Exception inner) : base(message, inner)
    {
        ValidationErrors = new List<string>();
    }
    public List<string> ValidationErrors { get;  }
}