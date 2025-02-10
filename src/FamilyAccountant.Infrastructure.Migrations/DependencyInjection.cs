using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyAccountant.Infrastructure.Migrations;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner =>
                runner.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ConfigureGlobalProcessorOptions(opt => opt.ProviderSwitches = "Force Quote=false")
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());
        
        RunMigrations(services);
        
        return services;
    }

    private static void RunMigrations(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}