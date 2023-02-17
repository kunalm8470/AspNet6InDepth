using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(new string[]
            {
                "Hello", "World"
            });
        }

        [HttpGet("dolongrunningoperation")]
        public async Task<IActionResult> DoLongRunningOperationAsync([FromQuery] int delay)
        {
            // Do some work here
            await Task.Delay(delay);

            return Ok();
        }
    }
}
