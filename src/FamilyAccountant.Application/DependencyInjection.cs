using System.Reflection;
using FamilyAccountant.Application.Services.Authentication;
using FamilyAccountant.Application.Services.Family;
using FamilyAccountant.Application.Services.Hasher;
using FamilyAccountant.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyAccountant.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LoginService>()
            .AddScoped<RefreshTokenService>()
            .AddScoped<RegisterService>()
            .AddScoped<FamilyService>()
            .AddScoped<IHasher, Sha256Hasher>()
            .AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}