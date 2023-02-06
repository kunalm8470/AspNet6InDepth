using Api.Interfaces;
using Api.Models;
using Dapper;
using System.Data;

namespace Api.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly SqlServerConnectionFactory _connectionFactory;

    public PersonRepository(SqlServerConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Person> GetPersonByIdAsync(Guid id)
    {
        string sql = @"
        SELECT *
        FROM [dbo].[Persons]
        WHERE [Id] = @id;";

        DynamicParameters parameters = new();

        parameters.Add("@id", id, DbType.Guid, ParameterDirection.Input);

        using IDbConnection connection = _connectionFactory.Connect();

        return await connection.QuerySingleAsync<Person>(sql, parameters);
    }

    public async Task<IEnumerable<Person>> GetPersonsPaginatedAsync(int page, int limit)
    {
        int skip = (page - 1) * limit;

        string sql = @"
        SELECT *
        FROM [dbo].[Persons]
        ORDER BY [Id]
        OFFSET @skip ROWS
        FETCH NEXT @limit ROWS ONLY;";

        DynamicParameters parameters = new();

        parameters.Add("@skip", skip, DbType.Int32, ParameterDirection.Input);

        parameters.Add("@limit", limit, DbType.Int32, ParameterDirection.Input);

        using IDbConnection connection = _connectionFactory.Connect();

        return await connection.QueryAsync<Person>(sql, parameters);
    }

    public async Task<Person> UpdatePersonAsync(Guid id, Person replacement)
    {
        string sql = @"
        UPDATE [dbo].[Persons]
        SET Title = @title,
        FirstName = @firstname,
        LastName = @lastname,
        Email = @email,
        Gender = @gender,
        Company = @company,
        Designation = @designation
        OUTPUT INSERTED.*
        WHERE Id = @id;";

        DynamicParameters parameters = new();

        parameters.Add("@id", id, DbType.Guid, ParameterDirection.Input);

        parameters.Add("@title", string.IsNullOrEmpty(replacement.Title) ? DBNull.Value : replacement.Title, DbType.String, ParameterDirection.Input, string.IsNullOrEmpty(replacement.Title) ? 0 : replacement.Title.Length);

        parameters.Add("@firstname", string.IsNullOrEmpty(replacement.FirstName) ? DBNull.Value : replacement.FirstName, DbType.String, ParameterDirection.Input, string.IsNullOrEmpty(replacement.FirstName) ? 0 : replacement.FirstName.Length);

        parameters.Add("@lastname", string.IsNullOrEmpty(replacement.LastName) ? DBNull.Value : replacement.LastName, DbType.String, ParameterDirection.Input, string.IsNullOrEmpty(replacement.LastName) ? 0 : replacement.LastName.Length);

        parameters.Add("@email", string.IsNullOrEmpty(replacement.Email) ? DBNull.Value : replacement.Email, DbType.String, ParameterDirection.Input, string.IsNullOrEmpty(replacement.Email) ? 0 : replacement.Email.Length);

        parameters.Add("@gender", string.IsNullOrEmpty(replacement.Gender) ? DBNull.Value : replacement.Gender, DbType.String, ParameterDirection.Input, string.IsNullOrEmpty(replacement.Gender) ? 0 : replacement.Gender.Length);

        parameters.Add("@company", string.IsNullOrEmpty(replacement.Company) ? DBNull.Value : replacement.Company, DbType.String, ParameterDirection.Input, string.IsNullOrEmpty(replacement.Company) ? 0 : replacement.Company.Length);

        parameters.Add("@designation", string.IsNullOrEmpty(replacement.Designation) ? DBNull.Value : replacement.Designation, DbType.String, ParameterDirection.Input, string.IsNullOrEmpty(replacement.Designation) ? 0 : replacement.Designation.Length);


        using IDbConnection connection = _connectionFactory.Connect();

        return await connection.QuerySingleAsync<Person>(sql, parameters);
    }
}
