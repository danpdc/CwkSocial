using CwkSocial.MinimalAPi.Contracts.Common;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Filters;

public class ModelValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ModelValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var model = context.Arguments
            .FirstOrDefault(a => a.GetType() == typeof(T)) as T;
        if (model is null)
        {
            var errorResponse = GenerateErrorResponse();
            errorResponse.Errors.Add("Request body not in correct format");
            return Results.Json(errorResponse, statusCode: errorResponse.StatusCode);
        }

        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errorResponse = GenerateErrorResponse();
            validationResult.Errors.ForEach(e => errorResponse.Errors.Add(e. ErrorMessage));
            return Results.Json(errorResponse, statusCode: errorResponse.StatusCode);
        }
        return await next(context);
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