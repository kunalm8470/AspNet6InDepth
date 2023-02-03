using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v3;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("3.0")]
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
}
