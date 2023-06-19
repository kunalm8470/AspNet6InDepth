using Api.Exceptions;
using Api.Models.Entities;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Api.Services;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    private readonly IRevokedRefreshTokenRepository _revokedRefreshTokenRepository;
    
    private readonly IKeyVaultService _keyVaultService;

    public AccountService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IRevokedRefreshTokenRepository revokedRefreshTokenRepository,
        IKeyVaultService keyVaultService
    )
    {
        _userRepository = userRepository;

        _refreshTokenRepository = refreshTokenRepository;
        
        _revokedRefreshTokenRepository = revokedRefreshTokenRepository;
        
        _keyVaultService = keyVaultService;
    }

    public async Task<SignupUserResponseDto> SignupUserAsync(SignupUserRequestDto newUser, CancellationToken cancellationToken = default)
    {
        try
        {
            User newAuthUser = new()
            {
                Id = Guid.NewGuid(),
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                DateOfBirth = newUser.DateOfBirth,
                Gender = newUser.Gender,
                Username = newUser.Username,
                PasswordHash = BCryptNet.HashPassword(newUser.Password)
            };

            return await _userRepository.AddUserAsync(newAuthUser.Id, newAuthUser.FirstName, newAuthUser.LastName, newAuthUser.Gender, newAuthUser.DateOfBirth, newAuthUser.Username, newAuthUser.Email, newAuthUser.PasswordHash, cancellationToken);
        }
        catch (SqlException ex) when ((ex.Number == 2601) || (ex.Number == 2627))
        {
            // Extracting the column name from the error message
            string errorMessage = ex.Message;
            int index = errorMessage.IndexOf("column '") + "column '".Length;
            int endIndex = errorMessage.IndexOf("'", index);
            string columnName = errorMessage[index..endIndex];

            throw new DuplicateUserException($"Duplicate value for '{columnName}'");
        }
    }

    public async Task<LoginUserResponseDto> LoginUserAsync(LoginUserRequestDto loginUser, CancellationToken cancellationToken = default)
    {
        User foundUser = await _userRepository.GetUserByUsernameAsync(loginUser.Username, cancellationToken);

        #region <<< Compare passwords >>>

        bool doesPasswordHashMatch = BCryptNet.Verify(loginUser.Password, foundUser.PasswordHash);

        if (!doesPasswordHashMatch)
        {
            throw new PasswordNotMatchingException();
        }

        #endregion

        #region <<< Generate access and refresh token pair >>>

        ECDsaSecurityKey securityKey = await _keyVaultService.GetEcdsaPrivateKeyAsync();

        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.EcdsaSha512);

        JwtSecurityTokenHandler tokenHandler = new();

        Claim[] accessTokenClaims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, foundUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("username", foundUser.Username),
            new Claim(JwtRegisteredClaimNames.Email, foundUser.Email),
            new Claim(JwtRegisteredClaimNames.Birthdate, foundUser.DateOfBirth.ToString("o")),
            new Claim(ClaimTypes.Role, "Manager")
        };

        JwtSecurityToken accessTokenObj = new(
            issuer: "authserver",
            audience: "resourceserver",
            claims: accessTokenClaims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: signingCredentials
        );

        string accessToken = tokenHandler.WriteToken(accessTokenObj);

        Claim[] refreshTokenClaims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, foundUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        JwtSecurityToken refreshTokenObj = new(
            issuer: "authserver",
            audience: "resourceserver",
            claims: refreshTokenClaims,
            expires: DateTime.UtcNow.AddDays(90),
            signingCredentials: signingCredentials
        );

        string refreshToken = tokenHandler.WriteToken(refreshTokenObj);

        #endregion

        #region <<< Persist refresh token in the database >>>

        await _refreshTokenRepository.UpsertRefreshTokenAsync(foundUser.Id, refreshToken, cancellationToken);

        #endregion

        return new LoginUserResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 15 * 60
        };
    }

    public async Task<LoginUserResponseDto> RegenerateAccessTokenAsync(RefreshTokenRequestDto refreshTokenRequest, CancellationToken cancellationToken = default)
    {
        // 1. Check whether refresh token is present in database or not ?
        _ = await _refreshTokenRepository.GetRefreshTokenAsync(refreshTokenRequest.RefreshToken, cancellationToken);

        // 2. Whether the refresh token was revoked or not ?
        RevokedRefreshToken revokedRefreshToken = await _revokedRefreshTokenRepository.GetRevokedRefreshTokenAsync(refreshTokenRequest.RefreshToken, cancellationToken);

        if (revokedRefreshToken is not null)
        {
            throw new RefreshTokenRevokedException("Refresh token is already revoked, Please login again to generate access token and refresh token pairs.");
        }

        // 3. Check whether refresh token is expired or not ? (Don't allow same refresh token after 90th day)
        ECDsaSecurityKey publicKey = await _keyVaultService.GetEcdsaPrivateKeyAsync();

        // 3.1 Validate the refresh token
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidIssuer = "authserver",
            ValidateIssuer = true,

            ValidAudience = "resourceserver",
            ValidateAudience = true,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero, // No grace period. Expire token at absolute time.
            IssuerSigningKey = publicKey
        };

        ClaimsPrincipal principal;
        
        try
        {
            principal = new JwtSecurityTokenHandler().ValidateToken(refreshTokenRequest.RefreshToken, tokenValidationParameters, out _);
        }
        catch (SecurityTokenInvalidLifetimeException)
        {
            throw new RefreshTokenExpiredException("Refresh token is expired.");
        }
        
        IEnumerable<Claim> claims = (principal.Identity as ClaimsIdentity).Claims;

        Guid userId = Guid.Parse(
            claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value
        );

        // Regenerate access token
        User foundUser = await _userRepository.GetUserByUserIdAsync(userId, cancellationToken);

        ECDsaSecurityKey securityKey = await _keyVaultService.GetEcdsaPrivateKeyAsync();

        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.EcdsaSha512);

        JwtSecurityTokenHandler tokenHandler = new();

        Claim[] accessTokenClaims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, foundUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("username", foundUser.Username),
            new Claim(JwtRegisteredClaimNames.Email, foundUser.Email),
            new Claim(JwtRegisteredClaimNames.Birthdate, foundUser.DateOfBirth.ToString("o")),
        };

        JwtSecurityToken accessTokenObj = new(
            issuer: "authserver",
            audience: "resourceserver",
            claims: accessTokenClaims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: signingCredentials
        );

        string accessToken = tokenHandler.WriteToken(accessTokenObj);

        return new LoginUserResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenRequest.RefreshToken,
            ExpiresIn = 15 * 60
        };
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        // 1. Check whether refresh token is present in database or not ?
        RefreshToken foundRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken, cancellationToken);

        // Revoke the refresh token
        await _revokedRefreshTokenRepository.RevokeRefreshTokenAsync(foundRefreshToken.UserId, refreshToken, cancellationToken);
    }
}
