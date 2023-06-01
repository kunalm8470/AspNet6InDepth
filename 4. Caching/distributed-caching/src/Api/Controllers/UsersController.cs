using Api.Exceptions;
using Api.Interfaces.Caching;
using Api.Interfaces.ThirdParty;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserClient _userClient;
    private readonly IRedisUserService _redisUserService;

    public UsersController(
		IUserClient userClient,
		IRedisUserService redisUserService
	)
	{
        _userClient = userClient;
        
        _redisUserService = redisUserService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUserByIdAsync([FromRoute] int id)
    {
        User found = default;

        try
        {
            found = await _redisUserService.GetUserFromCacheAsync(id);
        }
        catch (UserNotFoundException)
        {
            
        }

        /*
         * Check if user is not null,
         * then it's a "Cache-Hit"
         * we can directly return to the client.
        */
        if (found is not null)
        {
            return Ok(found);
        }

        /*
         * Below code will be executed in case of
         * "Cache-Miss".
         * 
         * It will query the third party API and store in
         * Redis Cache.
        */
        found = await _userClient.GetUserByIdAsync(id);

        await _redisUserService.AddUserToCacheAsync(id, found);

        return Ok(found);
    }
}
