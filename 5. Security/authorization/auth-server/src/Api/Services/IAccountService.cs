using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services;

public interface IAccountService
{
    Task<SignupUserResponseDto> SignupUserAsync(SignupUserRequestDto newUser, CancellationToken cancellationToken = default);

    Task<LoginUserResponseDto> LoginUserAsync(LoginUserRequestDto loginUser, CancellationToken cancellationToken = default);

    Task<LoginUserResponseDto> RegenerateAccessTokenAsync(RefreshTokenRequestDto refreshTokenRequest, CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
