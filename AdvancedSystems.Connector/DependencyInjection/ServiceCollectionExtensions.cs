using System;

using AdvancedSystems.Connector.Abstractions;
using AdvancedSystems.Connector.Options;
using AdvancedSystems.Connector.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdvancedSystems.Connector.DependencyInjection;

public static class ServiceCollectionExtensions
{
    #region DbConnectionService

    private static IServiceCollection AddDbConnectionService<T>(this IServiceCollection services) where T : class, IDbConnectionService
    {
        services.TryAdd(ServiceDescriptor.Singleton<IDbConnectionService, T>());
        return services;
    }

    private static IServiceCollection AddDbConnectionService<T, U>(this IServiceCollection services, Action<U> setupAction) where T : class, IDbConnectionService where U : DbSettings
    {
        services.AddOptions()
            .Configure(setupAction);

        services.AddDbConnectionService<T>();
        return services;
    }

    private static IServiceCollection AddDbConnectionService<T, U>(this IServiceCollection services, IConfiguration configuration) where T : class, IDbConnectionService where U : DbSettings
    {
        services.AddOptions<U>()
            .Bind(configuration.GetRequiredSection(Sections.DB_SETTINGS))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbConnectionService<T>();
        return services;
    }

    #endregion

    #region Microsoft SQL Server

    public static IServiceCollection AddMsSqlServerConnectionService(this IServiceCollection services, Action<DbSettings> setupAction)
    {
        services.AddDbConnectionService<MsSqlServerConnectionService, MsSqlServerSettings>(setupAction);
        return services;
    }

    public static IServiceCollection AddMsSqlServerConnectionService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnectionService<MsSqlServerConnectionService, MsSqlServerSettings>(configuration);
        return services;
    }

    #endregion
}
