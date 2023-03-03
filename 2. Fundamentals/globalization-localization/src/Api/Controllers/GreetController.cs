using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GreetController : ControllerBase
{
    private readonly IStringLocalizer<GreetController> _localizer;

    public GreetController(IStringLocalizer<GreetController> localizer)
    {
        _localizer = localizer;
    }

    [HttpGet]
    public ActionResult<string> GreetPerson([FromQuery] string name)
    {
        // Get the value from the resource file specified from the culture
        string greeting = _localizer["greeting", name];

        return Ok(greeting);
    }
}
