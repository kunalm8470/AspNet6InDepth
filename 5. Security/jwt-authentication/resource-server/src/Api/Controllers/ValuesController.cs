using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [Authorize]
    public ActionResult<string> GetProtectedResource()
    {
        return Ok("From protected route");
    }
}
