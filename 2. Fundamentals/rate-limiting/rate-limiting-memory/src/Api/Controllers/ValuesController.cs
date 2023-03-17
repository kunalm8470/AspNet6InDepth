using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<int> GetRandomInt()
        {
            Random r = new();

            int randomInt = r.Next(1, 11);

            return Ok(randomInt);
        }
    }
}
