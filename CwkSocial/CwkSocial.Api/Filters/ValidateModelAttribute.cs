using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
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
                apiError.Errors.Add(error.Value.ToString());
            }

            context.Result =  new JsonResult(apiError) {StatusCode = 400};
            // TO DO: Make sure Asp.NEt Core doesn't override our action result body
        }
    }
}