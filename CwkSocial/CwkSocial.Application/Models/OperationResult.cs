namespace CwkSocial.Application.Models;

public class OperationResult<T>
{
    public T Payload { get; set; }
    public bool IsError {get; set;}
    public List<Error> Errors {get; } = new List<Error>();
}