using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using yard.domain.ViewModels;

namespace yard.api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
/*        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]*/
        [NonAction]
        public IActionResult Response (ApiResponse response, string? actionName = null)
        {
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }

            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }

            if(response.StatusCode == 201)
            {
                return CreatedAtAction(nameof(actionName), response.Data);
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}
