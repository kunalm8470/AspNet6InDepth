using Api.Models.Entities;
using Api.Models.Responses;

namespace Api.Repositories;

public interface IUserRepository
{
    Task<SignupUserResponseDto> AddUserAsync(Guid id, string firstName, string lastName, string gender, DateTime dateOfBirth, string username, string email, string passwordHash, CancellationToken cancellationToken = default);

    Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<User> GetUserByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
