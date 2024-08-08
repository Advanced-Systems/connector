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
    public delegate string DecryptPassword(string cipher);

    #region DbConnectionService

    private static IServiceCollection AddDbConnectionService<T>(this IServiceCollection services) where T : class, IDatabaseConnectionService
    {
        services.TryAdd(ServiceDescriptor.Singleton<IDatabaseConnectionService, T>());
        return services;
    }

    private static IServiceCollection AddDbConnectionService<T, U>(this IServiceCollection services, Action<U> setupAction, DecryptPassword? decryptPassword = null) where T : class, IDatabaseConnectionService where U : DatabaseOptions
    {
        services.AddOptions()
            .Configure(setupAction)
            .PostConfigure<U>(options =>
            {
                if (decryptPassword != null)
                {
                    options.Password = decryptPassword(options.Password);
                }
            });

        services.AddDbConnectionService<T>();
        return services;
    }

    private static IServiceCollection AddDbConnectionService<T, U>(this IServiceCollection services, IConfiguration configuration, DecryptPassword? decryptPassword = null) where T : class, IDatabaseConnectionService where U : DatabaseOptions
    {
        services.AddOptions<U>()
            .Bind(configuration.GetRequiredSection(Sections.DATABASE))
            .PostConfigure(options =>
            {
                if (decryptPassword != null)
                {
                    options.Password = decryptPassword(options.Password);
                }
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbConnectionService<T>();
        return services;
    }

    #endregion

    #region Microsoft SQL Server

    public static IServiceCollection AddMsSqlServerConnectionService(this IServiceCollection services, Action<MsSqlServerSettings> setupAction, DecryptPassword? decryptPassword = null)
    {
        services.AddDbConnectionService<MsSqlServerConnectionService, MsSqlServerSettings>(setupAction, decryptPassword);
        return services;
    }

    public static IServiceCollection AddMsSqlServerConnectionService(this IServiceCollection services, IConfiguration configuration, DecryptPassword? decryptPassword = null)
    {
        services.AddDbConnectionService<MsSqlServerConnectionService, MsSqlServerSettings>(configuration, decryptPassword);
        return services;
    }

    #endregion
}
