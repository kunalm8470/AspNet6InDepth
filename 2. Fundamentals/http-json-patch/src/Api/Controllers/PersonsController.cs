using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;

        public PersonsController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsPaginatedAsync([FromQuery] int page, [FromQuery] int limit)
        {
            return Ok(await _personRepository.GetPersonsPaginatedAsync(page, limit));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Person>> GetPersonsByIdAsync([FromRoute] Guid id)
        {
            Person found = await _personRepository.GetPersonByIdAsync(id);

            if (found is null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<Person>> PatchPersonAsync([FromRoute] Guid id, [FromBody] JsonPatchDocument<Person> patchPerson)
        {
            Person found = await _personRepository.GetPersonByIdAsync(id);

            if (found is null)
            {
                return NotFound();
            }

            // Apply the request patch to the person object in memory
            patchPerson.ApplyTo(objectToApplyTo: found);

            // Handle any errors occured while applying patch
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }

            // Update the patched object to database
            Person patchedPerson = await _personRepository.UpdatePersonAsync(id, replacement: found);

            return Ok(patchedPerson);
        }
    }
}
