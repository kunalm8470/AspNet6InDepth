using Api.Dto;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public PersonsController(IPersonService personService, IHttpContextAccessor httpContextAccessor)
	{
        _personService = personService;

        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<List<Person>>> GetPersonsPaginatedAsync([FromQuery] int page, [FromQuery] int limit)
    {
        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        List<Person> people = await _personService.GetPersonsPaginatedAsync(page, limit, cancellationToken);

        return Ok(people);
    }

    [HttpPost]
    public async Task<IActionResult> AddPersonAsync([FromBody] AddPersonDto dto)
    {
        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        await _personService.AddPersonAsync(dto.Name, dto.Age, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }
}
