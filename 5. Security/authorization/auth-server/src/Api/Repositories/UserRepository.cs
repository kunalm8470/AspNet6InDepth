using Api.Database;
using Api.Exceptions;
using Api.Models.Entities;
using Api.Models.Responses;
using Dapper;
using System.Data;

namespace Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISqlServerConnectionFactory _connectionFactory;

    public UserRepository(ISqlServerConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<SignupUserResponseDto> AddUserAsync(Guid id, string firstName, string lastName, string gender, DateTime dateOfBirth, string username, string email, string passwordHash, CancellationToken cancellationToken = default)
    {
        string sql = @"INSERT INTO [auth].[Users] (Id, FirstName, LastName, Email, DateOfBirth, Gender, Username, PasswordHash)
        OUTPUT inserted.Id, inserted.FirstName, inserted.LastName, inserted.Email, inserted.Email, inserted.DateOfBirth, inserted.Gender, inserted.Username
        VALUES (@id, @firstName, @lastName, @email, @dateOfBirth, @gender, @username, @passwordHash);";

        DynamicParameters parameters = new();

        parameters.Add("@id", id, DbType.Guid, ParameterDirection.Input);

        parameters.Add("@firstName", firstName, DbType.String, ParameterDirection.Input, firstName.Length);

        parameters.Add("@lastName", lastName, DbType.String, ParameterDirection.Input, lastName.Length);

        parameters.Add("@email", email, DbType.String, ParameterDirection.Input, email.Length);

        parameters.Add("@dateOfBirth", dateOfBirth, DbType.DateTime, ParameterDirection.Input);

        parameters.Add("@gender", gender, DbType.String, ParameterDirection.Input, gender.Length);

        parameters.Add("@username", username, DbType.String, ParameterDirection.Input, username.Length);

        parameters.Add("@passwordHash", passwordHash, DbType.String, ParameterDirection.Input, passwordHash.Length);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        IDbConnection connection = _connectionFactory.Connect();

        return await connection.QuerySingleAsync<SignupUserResponseDto>(definition);
    }

    public async Task<User> GetUserByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        string sql = @"SELECT * FROM [auth].[users] WHERE [Id] = @id;";

        DynamicParameters parameters = new();

        parameters.Add("@id", userId, DbType.Guid, ParameterDirection.Input);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        IDbConnection connection = _connectionFactory.Connect();

        User found = await connection.QuerySingleAsync<User>(definition);

        if (found is null)
        {
            throw new UserNotFoundException($"User not found with id: {userId}");
        }

        return found;
    }

    public async Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        string sql = @"SELECT * FROM [auth].[users] WHERE [username] = @username;";

        DynamicParameters parameters = new();

        parameters.Add("@username", username, DbType.String, ParameterDirection.Input, username.Length);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        IDbConnection connection = _connectionFactory.Connect();

        User found = await connection.QuerySingleAsync<User>(definition);

        if (found is null)
        {
            throw new UserNotFoundException($"User not found with username: {username}");
        }

        return found;
    }

    
}
