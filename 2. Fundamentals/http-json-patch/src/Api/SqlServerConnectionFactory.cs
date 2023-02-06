using Microsoft.Data.SqlClient;
using System.Data;

namespace Api;

public class SqlServerConnectionFactory
{
    private readonly string _connectionString;

    public SqlServerConnectionFactory(string connectionString)
	{
        _connectionString = connectionString;
    }

    /// <summary>
    /// Open a connection from the connection pool
    /// </summary>
    /// <returns></returns>
    public IDbConnection Connect()
    {
        return new SqlConnection(_connectionString);
    }
}
