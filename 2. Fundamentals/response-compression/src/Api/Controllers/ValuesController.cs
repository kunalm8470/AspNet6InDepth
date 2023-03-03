using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Data>> Get()
    {
        List<Data> data = new(10000);

        for (int i = 0; i < 10000; i++)
        {
            data.Add(new Data
            {
                Key = Guid.NewGuid().ToString(),
                Value = (i + 1).ToString()
            });
        }

        return Ok(data);
    }
}
