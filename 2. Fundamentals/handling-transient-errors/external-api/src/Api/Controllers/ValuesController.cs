using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{

    [HttpGet("generaterandomint")]
    public ActionResult<int> ReturnRandom()
    {
        // (0, 10]
        Random rand = new();

        int generated = rand.Next(1, 11);

        if (generated > 5)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(generated);
    }
}
