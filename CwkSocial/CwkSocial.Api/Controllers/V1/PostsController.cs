using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController : Controller
    {
        [HttpGet]
        [Route(ApiRoutes.Posts.GetById)]
        public IActionResult GetById(int id)
        {
            return Ok();
        }
    }
}
