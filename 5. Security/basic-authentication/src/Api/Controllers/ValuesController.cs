using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValuesController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public ActionResult<int> GetNumberAsync([FromRoute] int id)
    {
        // Getting current user from HttpContext using HttpContextAccessor
        ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User;

        return id;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<string> GetStringAsync()
    {
        return "Hello world";
    }
}
