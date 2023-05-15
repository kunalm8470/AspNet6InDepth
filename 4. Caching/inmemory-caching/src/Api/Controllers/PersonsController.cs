using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public PersonsController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Person>>> GetPeopleAsync()
    {
        if (!_memoryCache.TryGetValue("people", out IReadOnlyList<Person> people))
        {
            people = Enumerable.Range(1, 100)
            .Select(_ =>
            {
                Bogus.Faker faker = new("en");

                return new Person
                {
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email,
                    Gender = faker.Person.Gender.ToString()
                };
            })
            .ToList()
            .AsReadOnly();

            await Task.Delay(2000);

            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(1)) // Cache will expire if inactive for 1 minute
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)); // Cache will expire in 5 minutes

            _memoryCache.Set("people", people, cacheEntryOptions);
        }

        return Ok(people);
    }
}
