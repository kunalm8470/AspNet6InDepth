using Api.Models.Entities;

namespace Api.Repositories;

public interface IRevokedRefreshTokenRepository
{
    Task<RevokedRefreshToken> GetRevokedRefreshTokenAsync(string revokedRefreshToken, CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(Guid userId, string revokedRefreshToken, CancellationToken cancellationToken = default);
}
