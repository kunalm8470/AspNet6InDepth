using Api.Models.Entities;

namespace Api.Repositories;

public interface IRefreshTokenRepository
{
    Task UpsertRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);

    Task<RefreshToken> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
