using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromServices] IHttpContextAccessor httpContextAccessor)
    {
        // HTTP Context is not thread safe
        // If multiple threads are trying to update some parameter in HttpContext
        // The most recent one will get updated.

        // Access the current request
        HttpRequest request = httpContextAccessor?.HttpContext?.Request;

        // E.g: Read the query parameters
        IQueryCollection queryParams = request?.Query;

        // Access the current response
        HttpResponse response = httpContextAccessor?.HttpContext?.Response;

        // Access the user's data
        ClaimsPrincipal user = httpContextAccessor?.HttpContext?.User;

        return Ok();
    }
}
