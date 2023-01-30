using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers;

[Route("api/[controller]")] // .NET 6
// [RoutePrefix("api/[controller]")] - .NET Framework
[ApiController]
public class ValuesController : ControllerBase
{
    /* 
     * Old way - .NET framework Web API
        
        [HttpGet]
        [Route("GetValues/{val1}/GetMoreValues/{val2}")]
        public IHttpActionResult Get(
            [FromUri] int val1,
            [FromUri] double val2,

            [FromUri] string q1,
            [FromUri] string q2
        )
        {
            return Ok();
        }
    */

    // New way - .NET 6
    [HttpGet("GetValues/{val1:int}/GetMoreValues/{val2:double}")]
    public IActionResult Get(
        [FromRoute] int val1,   // Newly added in .NET 6
        [FromRoute] double val2,

        [FromQuery] string q1, // Newly added in .NET 6
        [FromQuery] string q2
    )
    {
        return Ok();
    }

    [HttpGet("GetValues2/{value1}/GetMoreValues2/{value2}")]
    public IActionResult Get2(
        [FromRoute] RouteParamDto routeParam,
        [FromQuery] QueryParamDto queryParam
    )
    {
        return Ok();
    }

    /*
        By this way we can add multiple routes
        to a Controller action method

        To make it a default route we use empty string
    */
    [HttpGet("")]
    [HttpGet("Index")]
    public ActionResult DefaultRoute()
    {
        var response = new
        {
            message = "From default route"
        };

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<BodyDto> Post([FromBody] BodyDto bodyDto)
    {
        return Ok(bodyDto);
    }

    // Read the body manually
    [HttpPost("manual-read")]
    public async ValueTask<ActionResult<BodyDto>> Post2([FromServices] IHttpContextAccessor httpContextAccessor)
    {
        using StreamReader stream = new(stream: httpContextAccessor?.HttpContext?.Request.Body);

        string bodyStr = await stream.ReadToEndAsync();

        BodyDto dto = JsonSerializer.Deserialize<BodyDto>(bodyStr, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return Ok(dto);
    }

    // Read from form (sent as key-value pairs)
    // Content-Type: application/x-www-form-urlencoded
    [HttpPost("form")]
    public ActionResult<BodyDto> Post3([FromForm] BodyDto formDto) // Added in .NET 6
    {
        return Ok(formDto);
    }

    // Read from header
    [HttpPost("header")]
    public ActionResult<string> Post4([FromHeader] string customHeader) // Added in .NET 6
    {
        return Ok(customHeader);
    }
}
