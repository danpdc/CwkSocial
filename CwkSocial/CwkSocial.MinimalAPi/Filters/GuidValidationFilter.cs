using CwkSocial.MinimalAPi.Contracts.Common;

namespace CwkSocial.MinimalAPi.Filters;

public class GuidValidationFilter : IEndpointFilter
{
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        foreach (var keyValuePair in context.HttpContext.Request.RouteValues)
        {
            if (keyValuePair.Key.EndsWith("Id") || keyValuePair.Key == "id")
            {
                var isValid = IsValidGuid(keyValuePair.Value.ToString());
                if (!isValid)
                {
                    var errorResponse = GenerateErrorResponse();
                    errorResponse.Errors.Add("Identifier not in correct GUID format");
                    return Results.Json(errorResponse, statusCode: errorResponse.StatusCode);
                }
            }
        }
        
        return await next(context);
    }

    private bool IsValidGuid(string id)
    {
        return Guid.TryParse(id, out var value);
    }
    
    protected ErrorResponse GenerateErrorResponse()
    {
        var apiError = new ErrorResponse();
        apiError.StatusCode = 400;
        apiError.StatusPhrase = "Bad request";
        apiError.Timestamp = DateTime.Now;

        return apiError;
    }
}