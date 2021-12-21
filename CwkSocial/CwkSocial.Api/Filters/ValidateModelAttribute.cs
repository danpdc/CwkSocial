using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var apiError = new ErrorResponse();
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            var errors = context.ModelState.AsEnumerable();

            foreach (var error in errors)
            {
                foreach (var inner in error.Value.Errors)
                {
                    apiError.Errors.Add(inner.ErrorMessage);
                }
            }

            context.Result = new BadRequestObjectResult(apiError);
            // TO DO: Make sure Asp.NEt Core doesn't override our action result body
        }
    }
}