using Api.Exceptions;
using Api.Interfaces.Caching;
using Api.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Api.Caching;

public class RedisUserService : IRedisUserService
{
    private readonly IDatabase _db;

	public RedisUserService(IConnectionMultiplexer connectionMultiplexer)
	{
		// Connect to the 0th database in Redis
		_db = connectionMultiplexer.GetDatabase();
	}

    public async Task AddUserToCacheAsync(int id, User user)
    {
        RedisKey key = $"users:{id}";

        RedisValue value = JsonConvert.SerializeObject(user);

        TimeSpan expiry = TimeSpan.FromSeconds(30);

        await _db.StringSetAsync(key, value, expiry);
    }

    public async Task<User> GetUserFromCacheAsync(int id)
    {
        RedisKey key = $"users:{id}";

        RedisValue cacheEntry = await _db.StringGetAsync(key);

        if (!cacheEntry.HasValue)
        {
            throw new UserNotFoundException("User not found with id: ${id}");
        }

        User found = JsonConvert.DeserializeObject<User>(
            (string)cacheEntry
        );

        return found;
    }
}
