using Api.Models;

namespace Api.Interfaces.ThirdParty;

public interface IUserClient
{
    Task<User> GetUserByIdAsync(int id);
}
