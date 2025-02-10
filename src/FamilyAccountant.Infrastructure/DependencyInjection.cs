using FamilyAccountant.Application.DbConnection;
using FamilyAccountant.Application.Services;
using FamilyAccountant.Domain.Repositories;
using FamilyAccountant.Infrastructure.DbConnection;
using FamilyAccountant.Infrastructure.Migrations;
using FamilyAccountant.Infrastructure.Options;
using FamilyAccountant.Infrastructure.Repositories;
using FamilyAccountant.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyAccountant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new ApplicationException("Missing connection string");

        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IFamilyRepository, FamilyRepository>()
            .AddSingleton<ITokenService, TokenService>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .Configure<JwtSettings>(configuration.GetSection("JwtSettings"))
            .AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString))
            .AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>()
            .AddPersistence(connectionString);

        return services;
    }
}