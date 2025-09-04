

using Microsoft.Extensions.Options;
using Npgsql;
using SharedKernel.Core.Persistence;
using SharedKernel.Infrastructure.Database;
using System.Data;

namespace DotnetTestTask.Infrastructure.Data;

internal class PostgresConnectionFactory 
    : IDbConnectionFactory
    , IDisposable
{
    private readonly IOptions<DatabaseOptions> _options;

    public PostgresConnectionFactory(IOptions<DatabaseOptions> options)
    {
        _options = options;
    }

    public IDbConnection GetConnection()
    {
        var connection = new NpgsqlConnection(_options.Value.ConnectionString);
        connection.Open();
        return connection;
    }

    public void Return(IDbConnection connection)
    {
        if (connection == null)
            return;

        if (connection.State != ConnectionState.Closed)
            connection.Close();

        connection.Dispose();
    }

    public void Dispose()
    {
 
    }
}