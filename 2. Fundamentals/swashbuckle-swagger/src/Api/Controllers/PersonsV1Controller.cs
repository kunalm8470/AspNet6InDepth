using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class PersonsV1Controller : ControllerBase
{
    /// <summary>
    /// <para>
    ///     Return a person randomly
    ///     
    ///     lorem ipsum
    ///     lorem ipsum
    ///     lorem ipsum
    /// </para>
    /// </summary>
    /// <returns>Return an object denoting a person having 3 fields Id, Name and Age.</returns>
    [HttpGet]
    public ActionResult<Person> GetPerson()
    {
        Person person = new()
        {
            Id = 1,
            Name = "John Doe",
            Age = 10
        };

        return Ok(person);
    }


    // Alternative way if you don't want to use XML
    [HttpPut(Name = "UpdatePersonV1")]
    [SwaggerOperation(
        Summary = "Update a person",
        Description = "Update a person by concatenating the name twice",
        OperationId = "updatePerson",
        Tags = new string[]
        {
            "Persons"
        }
    )]
    [SwaggerResponse(200, "The person was updated", Type = typeof(Person))]
    [SwaggerResponse(400, "The request was invalid", Type = typeof(IDictionary<string, object>))]
    public ActionResult<Person> UpdatePerson([FromBody, SwaggerRequestBody("Person to update")] Person payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        payload.Name += payload.Name;

        return Ok(payload);
    }



    [HttpGet("demo")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Result()
    {
        return Ok("demo");
    }
}
