using Api.Interfaces;
using Api.Models;
using Api.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsRepository _repository;

        private readonly HttpContext _context;

        public PersonsController(
            IPersonsRepository repository,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _repository = repository;
            _context = httpContextAccessor.HttpContext;
        }

        [HttpGet("offset")]
        public async Task<ActionResult<IReadOnlyList<Person>>> GetPersonsAsync([FromQuery] int page, [FromQuery] int limit)
        {
            CancellationToken requestCancellation = _context.RequestAborted;

            IReadOnlyList<Person> persons = await _repository.GetPersonOffsetPaginationAsync(page, limit, requestCancellation);

            return Ok(persons);
        }

        [HttpGet("keyset")]
        public async Task<IActionResult> GetPersonsAsync([FromQuery] string searchAfter, [FromQuery] int limit)
        {
            CancellationToken requestCancellation = _context.RequestAborted;

            Guid? searchAfterVal = default;

            DateTime? d = default;

            if (!string.IsNullOrEmpty(searchAfter))
            {
                string[] searchAfterParams = searchAfter.Split('_');

                searchAfterVal = Guid.Parse(searchAfterParams[0]);

                d = DateTime.ParseExact(searchAfterParams[1], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                d = DateTime.SpecifyKind(d.Value, DateTimeKind.Utc);
            }

            IReadOnlyList<Person> persons = await _repository.GetPersonKeysetPaginationAsync(searchAfterVal, d, limit, requestCancellation);

            if (!persons.Any())
            {
                return Ok(new
                {
                    searchAfter = default(Guid?),
                    data = persons
                });
            }

            Guid nextSearchAfterVal = persons[^1].Id;

            DateTime nextSearchAfterTime = persons[^1].CreatedAt;

            nextSearchAfterTime = DateTime.SpecifyKind(nextSearchAfterTime, DateTimeKind.Utc);

            return Ok(new
            {
                searchAfter = $"{nextSearchAfterVal}_{nextSearchAfterTime:yyyy-MM-ddTHH:mm:ss}",
                data = persons
            });
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Person>> GetPersonsAsync([FromRoute] Guid id)
        {
            CancellationToken requestCancellation = _context.RequestAborted;

            Person person = await _repository.GetPersonByIdAsync(id, requestCancellation);

            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> AddPersonAsync([FromBody] AddPersonDto dto)
        {
            CancellationToken requestCancellation = _context.RequestAborted;

            Person person = new()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age
            };

            Person added = await _repository.AddPersonAsync(person, requestCancellation);

            return StatusCode(StatusCodes.Status201Created, added);
        }

        [HttpPost("{id:guid}/address")]
        public async Task<IActionResult> AddPersonAddressAsync(
            [FromRoute] Guid id,
            [FromBody] AddPersonAddressDto dto
        )
        {
            CancellationToken requestCancellation = _context.RequestAborted;

            _ = await _repository.GetPersonByIdAsync(id, requestCancellation);

            await _repository.AddAddressToPersonAsync(id, dto.Address, requestCancellation);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeletePersonAsync([FromRoute] Guid id)
        {
            CancellationToken requestCancellation = _context.RequestAborted;

            _ = await _repository.GetPersonByIdAsync(id, requestCancellation);

            await _repository.DeletePersonAsync(id, requestCancellation);

            return NoContent();
        }
    }
}
