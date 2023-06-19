using Api.Database;
using Api.Exceptions;
using Api.Models.Entities;
using Dapper;
using System.Data;

namespace Api.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ISqlServerConnectionFactory _connectionFactory;

    public RefreshTokenRepository(ISqlServerConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        string sql = @"SELECT * FROM [auth].[RefreshTokens] WHERE Token = @token;";

        DynamicParameters parameters = new();

        parameters.Add("@token", refreshToken, DbType.String, ParameterDirection.Input, refreshToken.Length);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        IDbConnection connection = _connectionFactory.Connect();

        RefreshToken foundToken = await connection.QuerySingleOrDefaultAsync<RefreshToken>(definition);

        if (foundToken is null)
        {
            throw new RefreshTokenNotFoundException("Refresh token not found");
        }

        return foundToken;
    }

    public async Task UpsertRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        string sql = @"SELECT * FROM [auth].[RefreshTokens] WHERE UserId = @userId;";

        DynamicParameters refreshTokenParameters = new();

        refreshTokenParameters.Add("@userId", userId, DbType.Guid, ParameterDirection.Input);

        CommandDefinition definition = new(sql, refreshTokenParameters, cancellationToken: cancellationToken);

        IDbConnection connection = _connectionFactory.Connect();

        RefreshToken foundToken = await connection.QuerySingleOrDefaultAsync<RefreshToken>(definition);

        if (foundToken is null)
        {
            sql = @"INSERT INTO [auth].[RefreshTokens] (UserId, Token) VALUES (@userId, @token);";

            refreshTokenParameters.Add("@token", refreshToken, DbType.String, ParameterDirection.Input, refreshToken.Length);

            definition = new(sql, refreshTokenParameters, cancellationToken: cancellationToken);

            await connection.ExecuteAsync(definition);
        }
        else
        {
            sql = @"UPDATE [auth].[RefreshTokens] SET Token = @token WHERE UserId = @userId;";

            refreshTokenParameters.Add("@token", refreshToken, DbType.String, ParameterDirection.Input, refreshToken.Length);

            definition = new(sql, refreshTokenParameters, cancellationToken: cancellationToken);

            await connection.ExecuteAsync(definition);
        }
    }
}
