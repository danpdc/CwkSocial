using System.Reflection.Metadata;
using CwkSocial.Application.Enums;

namespace CwkSocial.Application.Models;

public class OperationResult<T>
{
    public T Payload { get; set; }
    public bool IsError {get; private set;}
    public List<Error> Errors {get; } = new List<Error>();

    /// <summary>
    /// Adds an error to the Error list and sets the IsError flag to true
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public void AddError(ErrorCode code, string message)
    {
        HandleError(code, message);
    }

    /// <summary>
    /// Adds a default error to the Error list with the error code UnknownError
    /// </summary>
    /// <param name="message"></param>
    public void AddUnknownError(string message)
    {
        HandleError(ErrorCode.UnknownError, message);
    }

    /// <summary>
    /// Sets the IsError flag to default (false)
    /// </summary>
    public void ResetIsErrorFlag()
    {
        IsError = false;
    }

    #region Private methods

    private void HandleError(ErrorCode code, string message)
    {
        Errors.Add(new Error {Code = code, Message = message});
        IsError = true;
    }

    #endregion
}