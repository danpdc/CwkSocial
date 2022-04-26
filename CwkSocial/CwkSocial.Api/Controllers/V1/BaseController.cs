namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class BaseController : ControllerBase
{
    private IMediator _mediatorInstance;
    private IMapper _mapperInstance;
    protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
    protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();

    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        var apiError = new ErrorResponse();

        if (errors.Any(e => e.Code == ErrorCode.NotFound))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

            apiError.StatusCode = 404;
            apiError.StatusPhrase = "Not Found";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return NotFound(apiError);
        }

        apiError.StatusCode = 400;
        apiError.StatusPhrase = "Bad request";
        apiError.Timestamp = DateTime.Now;
        errors.ForEach(e => apiError.Errors.Add(e.Message));
        return StatusCode(400, apiError);
    }
}