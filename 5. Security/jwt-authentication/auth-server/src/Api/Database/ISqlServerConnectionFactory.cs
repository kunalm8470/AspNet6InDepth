using System.Data;

namespace Api.Database;

public interface ISqlServerConnectionFactory
{
    IDbConnection Connect();
}
