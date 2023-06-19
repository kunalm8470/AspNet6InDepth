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

    [HttpGet]
    [Authorize]
    public ActionResult<string> GetProtectedResource()
    {
        return Ok("From protected route");
    }

    [HttpGet("adminroute")]
    [Authorize(Roles = "Admin")]
    public ActionResult<string> GetAdminRouteMessage()
    {
        return Ok("From admin route!");
    }

    [HttpGet("adminormanagerroute")]
    [Authorize(Roles = "Admin,Manager")] // Role1, Role2 comma separated is interpreted as Role1 || Role2
    public ActionResult<string> GetAdminOrManagerRouteMessage()
    {
        string loggedInRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(loggedInRole))
        {
            return Forbid();
        }

        return Ok($"From {loggedInRole} route!");
    }

    // Is interpreted as Role1 && Role2
    [HttpGet("adminandmanagerroute")]
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Manager")]
    public ActionResult<string> GetAdminAndManagerRouteMessage()
    {
        string loggedInRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(loggedInRole))
        {
            return Forbid();
        }

        return Ok($"From {loggedInRole} route!");
    }
}
