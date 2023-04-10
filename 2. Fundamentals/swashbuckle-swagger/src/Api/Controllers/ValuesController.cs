using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
public class ValuesController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Tags = new string[]
        {
            "Persons2"
        }    
    )]
    public ActionResult<string> Get()
    {
        return Ok("Hello world");
    }
}
