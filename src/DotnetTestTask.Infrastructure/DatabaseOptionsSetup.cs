using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharedKernel.Infrastructure.Database;

namespace Product.Infrastructure.Data;

internal class DatabaseOptionsSetup
    : IConfigureOptions<DatabaseOptions>
{
    private const string ConfigurationSectionName = "DatabaseOptions";
    private const string DatabaseName = "TestDb";

    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString(DatabaseName);
        
        if (connectionString is not null)
        {
            options.ConnectionString = connectionString;
        }

        _configuration.GetSection(ConfigurationSectionName).Bind(options);

    }
}
