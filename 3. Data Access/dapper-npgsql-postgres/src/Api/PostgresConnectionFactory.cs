using Npgsql;

namespace Api;

public class PostgresConnectionFactory
{
    public NpgsqlDataSource DataSource { get; }

	public PostgresConnectionFactory(string connectionString)
	{
        // Create a datasource
        DataSource = NpgsqlDataSource.Create(connectionString);
	}
}
