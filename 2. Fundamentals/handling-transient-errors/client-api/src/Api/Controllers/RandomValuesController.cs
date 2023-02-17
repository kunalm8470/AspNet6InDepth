using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RandomValuesController : ControllerBase
{
    private readonly IRandomValueService _randomValueService;

    public RandomValuesController(IRandomValueService randomValueService)
    {
        _randomValueService = randomValueService;
    }

    [HttpGet]
    public async Task<ActionResult<int>> ReturnRandomInteger()
    {
        int generated = await _randomValueService.GetRandomIntegerAsync();

        return Ok(generated);
    }
}
