using Api.Models;

namespace Api.Interfaces.Caching;

public interface IRedisUserService
{
    Task<User> GetUserFromCacheAsync(int id);
    
    Task AddUserToCacheAsync(int id, User user);
}
