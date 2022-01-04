using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.Api.Filters;

public class CwkSocialExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var apiError = new ErrorResponse
        {
            StatusCode = 500,
            StatusPhrase = "Internal Server Error",
            Timestamp = DateTime.Now
        };
        apiError.Errors.Add(context.Exception.Message);

        context.Result = new JsonResult(apiError) { StatusCode = 500};
    }
}