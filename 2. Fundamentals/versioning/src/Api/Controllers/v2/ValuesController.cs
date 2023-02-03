using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v2;

[Route("api/[controller]")]
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
[ApiVersion("1.1", Deprecated = true)]
public class ValuesController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValuesController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        ApiVersion version = _httpContextAccessor.HttpContext.GetRequestedApiVersion();

        return $"From v{version} Controller";
    }

    [HttpGet]
    [MapToApiVersion("1.1")] // v1.1 specific action for GET api/values endpoint
    public ActionResult<IEnumerable<string>> GetV1_1()
    {
        return new string[] { "version 1.1 value 1", "version 1.1 value2 " };
    }
}
