using Api.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("{id:Guid}")]
        [DebugResourceFilter(-1)] // This will be executed first while going in
        [Debug2ResourceFilter(2)] // This will be executed second
        [Debug3ResourceFilter(30)] // This will be executed last
        [ServiceFilter(typeof(EnsureNonEmptyGuidAsyncActionFilter))] // 3. Decorate with ServiceFilter to do dependency injection
        public IActionResult GetById([FromRoute] Guid id, [FromQuery] Guid id2)
        {
            return Ok(new
            {
                id,
                id2
            });
        }
    }
}
