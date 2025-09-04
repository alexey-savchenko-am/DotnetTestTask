using DotnetTestTask.Infrastructure.Data;
using DotnetTestTask.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Product.Infrastructure.Data;
using Project.Core.TreeAggregate;
using SharedKernel.Core.Persistence;
using SharedKernel.Infrastructure.Database;
using SharedKernel.Persistence;

namespace DotnetTestTask.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();
        services.AddSingleton<IDbConnectionFactory, PostgresConnectionFactory>();
      
        services.AddDbContext<DbContext, AppDbContext>((provider, builder) =>
        {
            var options = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            builder.UseNpgsql(options.ConnectionString, actions =>
            {
                actions.EnableRetryOnFailure(options.MaxRetryCount);
                actions.CommandTimeout(options.CommandTimeout);
            });
            builder.EnableDetailedErrors(options.EnableDetailedErrors);
            builder.EnableSensitiveDataLogging(options.EnableSensitiveDataLogging);
        });

        services.AddScoped<IDatabaseInitializer, TestDbContextSeed>();

        services.AddScoped<ISession, Session>();
        services.AddScoped<INodeRepository, NodeRepository>();

        return services;
    }
}
