using Api.Exceptions;
using Api.Interfaces;
using Api.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace Api.Repositories;

public class PersonsRepository : IPersonsRepository
{
    private readonly PostgresConnectionFactory _connectionFactory;

    public PersonsRepository(PostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAddressToPersonAsync(Guid id, string address, CancellationToken cancellationToken)
    {
        try
        {
            string sql = @"INSERT INTO
            addresses (person_id, address)
            VALUES (@personId, @address);";

            DynamicParameters parameters = new();

            parameters.Add("@personId", id, DbType.Guid, ParameterDirection.Input);

            parameters.Add("@address", address, DbType.String, ParameterDirection.Input, address.Length);

            CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

            // Pick the connection from the connection pool
            using IDbConnection connection = _connectionFactory.DataSource.CreateConnection();

            await connection.ExecuteAsync(definition);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505") // Catch duplicate key error
        {
            throw new DuplicateEntityException($"Address already added for person with id: {id}");
        }
    }

    public async Task<Person> AddPersonAsync(Person person, CancellationToken cancellationToken)
    {
        string sql = @"
        INSERT INTO 
        persons AS p (first_name, last_name, age)
        VALUES (@firstName, @lastName, @age)
        RETURNING p.id as Id,
        p.first_name as FirstName,
        p.last_name as LastName,
        p.age as Age,
        p.created_at as CreatedAt,
        p.updated_at as UpdatedAt;";

        DynamicParameters parameters = new();

        parameters.Add("@firstName", person.FirstName, DbType.String, ParameterDirection.Input, person.FirstName.Length);

        parameters.Add("@lastName", person.LastName, DbType.String, ParameterDirection.Input, person.LastName.Length);

        parameters.Add("@age", person.Age, DbType.Int32, ParameterDirection.Input);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        // Pick the connection from the connection pool
        using IDbConnection connection = _connectionFactory.DataSource.CreateConnection();

        return await connection.QuerySingleAsync<Person>(definition);
    }

    public async Task DeletePersonAsync(Guid id, CancellationToken cancellationToken)
    {
        string sql = @"DELETE
        FROM persons p
        WHERE p.id=@personId;";

        DynamicParameters parameters = new();

        parameters.Add("@personId", id, DbType.Guid, ParameterDirection.Input);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        // Pick the connection from the connection pool
        using IDbConnection connection = _connectionFactory.DataSource.CreateConnection();

        await connection.ExecuteAsync(definition);
    }

    public async Task<Person> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        string sql = @"SELECT 
        p.id as Id,
        p.first_name as FirstName,
        p.last_name as LastName,
        p.age as Age,
        p.created_at as CreatedAt,
        p.updated_at as UpdatedAt,
        a.address as Address,
        a.id as Id,
        a.person_id as PersonId,
        a.created_at as CreatedAt,
        a.updated_at as UpdatedAt
        FROM persons p
        LEFT JOIN addresses a
        ON a.person_id = p.id
        WHERE p.id = @personId;";

        DynamicParameters parameters = new();

        parameters.Add("@personId", id, DbType.Guid, ParameterDirection.Input);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        // Pick the connection from the connection pool
        using IDbConnection connection = _connectionFactory.DataSource.CreateConnection();

        Person found = (await connection.QueryAsync<Person, PersonAddress, Person>(definition, map: (person, address) =>
        {
            person.Address = address;

            return person;
        }, splitOn: "address")).FirstOrDefault();

        if (found is null)
        {
            throw new NotFoundException($"Person with {id} not found!");
        }

        return found;
    }

    public async Task<IReadOnlyList<Person>> GetPersonOffsetPaginationAsync(int page, int limit, CancellationToken cancellationToken)
    {
        int skip = (page - 1) * limit;

        string sql = @"SELECT 
        p.id as Id,
        p.first_name as FirstName,
        p.last_name as LastName,
        p.age as Age,
        p.created_at as CreatedAt,
        p.updated_at as UpdatedAt,
        a.address as Address,
        a.id as Id,
        a.person_id as PersonId,
        a.created_at as CreatedAt,
        a.updated_at as UpdatedAt
        FROM persons p
        LEFT JOIN addresses a
        ON a.person_id = p.id
        ORDER BY p.created_at DESC
        LIMIT @limit
        OFFSET @offset;";

        DynamicParameters parameters = new();

        parameters.Add("@limit", limit, DbType.Int32, ParameterDirection.Input);

        parameters.Add("@offset", skip, DbType.Int32, ParameterDirection.Input);

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        // Pick the connection from the connection pool
        using IDbConnection connection = _connectionFactory.DataSource.CreateConnection();

        Dictionary<Guid, Person> historyDictionary = new();

        List<Person> persons = (await connection.QueryAsync<Person, PersonAddress, Person>(definition, map: (person, address) =>
        {
            person.Address = address;

            return person;
        }, splitOn: "address"))
        .ToList();

        return persons.AsReadOnly();
    }

    public async Task<IReadOnlyList<Person>> GetPersonKeysetPaginationAsync(Guid? searchAfter, DateTime? searchAfterTime, int limit, CancellationToken cancellationToken)
    {
        string sql;

        DynamicParameters parameters = new();

        parameters.Add("@limit", limit, DbType.Int32, ParameterDirection.Input);


        if (searchAfter is not null)
        {
            sql = @"SELECT 
            p.id as Id,
            p.first_name as FirstName,
            p.last_name as LastName,
            p.age as Age,
            p.created_at as CreatedAt,
            p.updated_at as UpdatedAt,
            a.address as Address,
            a.id as Id,
            a.person_id as PersonId,
            a.created_at as CreatedAt,
            a.updated_at as UpdatedAt
            FROM persons p
            LEFT JOIN addresses a
            ON a.person_id = p.id
            WHERE p.created_at < @searchAfterTime
            OR(
                p.id < @searchAfter
                AND p.created_at < @searchAfterTime
            )
            ORDER BY p.created_at DESC
            LIMIT 1;";

            parameters.Add("@searchAfterTime", searchAfterTime.Value, DbType.DateTime, ParameterDirection.Input);

            parameters.Add("@searchAfter", searchAfter.Value, DbType.Guid, ParameterDirection.Input);
        }
        else
        {
            sql = @"SELECT 
            p.id as Id,
            p.first_name as FirstName,
            p.last_name as LastName,
            p.age as Age,
            p.created_at as CreatedAt,
            p.updated_at as UpdatedAt,
            a.address as Address,
            a.id as Id,
            a.person_id as PersonId,
            a.created_at as CreatedAt,
            a.updated_at as UpdatedAt
            FROM persons p
            LEFT JOIN addresses a
            ON a.person_id = p.id
            ORDER BY p.created_at DESC
            LIMIT @limit;";
        }

        CommandDefinition definition = new(sql, parameters, cancellationToken: cancellationToken);

        // Pick the connection from the connection pool
        using IDbConnection connection = _connectionFactory.DataSource.CreateConnection();

        List<Person> persons = (await connection.QueryAsync<Person, PersonAddress, Person>(definition, map: (person, address) =>
        {
            person.Address = address;

            return person;
        }, splitOn: "address"))
        .ToList();

        return persons.AsReadOnly();
    }
}
