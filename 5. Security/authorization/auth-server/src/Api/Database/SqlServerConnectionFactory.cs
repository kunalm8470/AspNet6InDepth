using Microsoft.Data.SqlClient;
using System.Data;

namespace Api.Database;

public class SqlServerConnectionFactory : ISqlServerConnectionFactory
{
    private readonly string _connectionString;

    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Connect()
    {
        return new SqlConnection(_connectionString);
    }
}
