using Application.v1.Persons.Commands.AddPerson;
using Application.v1.Persons.Commands.DeletePerson;
using Application.v1.Persons.Commands.UpdatePerson;
using Application.v1.Persons.Queries.GetAllPeople;
using Application.v1.Persons.Queries.GetPersonById;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PersonsController(
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor
    )
	{
        _mediator = mediator;
        
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetAllPeopleAsync()
    {
        // Come to know if request is cancelled or not
        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        IEnumerable<Person> people = await _mediator.Send(new GetAllPeopleQuery(), cancellationToken);

        return Ok(people);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Person>> GetPersonByIdAsync([FromRoute] GetPersonByIdQuery query)
    {
        // Come to know if request is cancelled or not
        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        Person found = await _mediator.Send(query, cancellationToken);

        return Ok(found);
    }

    [HttpPost]
    public async Task<IActionResult> AddPersonAsync([FromBody] AddPersonCommand command)
    {
        // Come to know if request is cancelled or not
        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        await _mediator.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Person>> UpdatePersonAsync([FromRoute] Guid id, [FromBody] UpdatePersonCommand command)
    {
        if (id != command.Id)
        {
            throw new PersonIdMismatchException("Id does not match the payload!");
        }

        // Come to know if request is cancelled or not
        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        Person updated = await _mediator.Send(command, cancellationToken);

        return Ok(updated);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePersonAsync([FromRoute] DeletePersonCommand command)
    {
        // Come to know if request is cancelled or not
        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
