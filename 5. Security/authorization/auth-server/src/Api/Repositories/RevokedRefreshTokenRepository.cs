using Api.Database;
using Api.Models.Entities;
using Dapper;
using System.Data;

namespace Api.Repositories;

public class RevokedRefreshTokenRepository : IRevokedRefreshTokenRepository
{
    private readonly ISqlServerConnectionFactory _connectionFactory;

    public RevokedRefreshTokenRepository(ISqlServerConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RevokedRefreshToken> GetRevokedRefreshTokenAsync(string revokedRefreshToken, CancellationToken cancellationToken = default)
    {
        string sql = @"SELECT * FROM [auth].[RevokedRefreshTokens] WHERE Token = @token;";

        DynamicParameters parameters = new();

        parameters.Add("@token", revokedRefreshToken, DbType.String, ParameterDirection.Input, revokedRefreshToken.Length);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        IDbConnection connection = _connectionFactory.Connect();

        return await connection.QuerySingleOrDefaultAsync<RevokedRefreshToken>(definition);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId, string revokedRefreshToken, CancellationToken cancellationToken = default)
    {
        string sql = @"INSERT INTO [auth].[RevokedRefreshTokens] (UserId, Token) VALUES (@userId, @token);";

        DynamicParameters parameters = new();

        parameters.Add("@userId", userId, DbType.Guid, ParameterDirection.Input);

        parameters.Add("@token", revokedRefreshToken, DbType.String, ParameterDirection.Input, revokedRefreshToken.Length);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        IDbConnection connection = _connectionFactory.Connect();

        await connection.ExecuteAsync(definition);
    }
}
