namespace CwkSocial.Application.Enums;

public enum ErrorCode
{
    NotFound = 404,
    ServerError = 500,
    
    //Validation errors should be in the range 100 - 199
    ValidationError = 101,
    
    //Infrastructure errors should be in the range 200-299
    IdentityUserAlreadyExists = 201,
    IdentityCreationFailed = 202,
    IdentityUserDoesNotExist = 203,
    IncorrectPassword = 204,
    
    UnknownError = 999
    
    
}