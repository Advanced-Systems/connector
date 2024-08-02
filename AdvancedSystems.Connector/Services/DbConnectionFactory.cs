using System;

using AdvancedSystems.Connector.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Connector.Services;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly ILogger<DbConnectionFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DbConnectionFactory(ILogger<DbConnectionFactory> logger, IServiceProvider serviceProvider)
    {
        this._logger = logger;
        this._serviceProvider = serviceProvider;
    }

    public IDbConnectionService Create(Provider provider)
    {
        return provider switch
        {
            Provider.MsSql => this._serviceProvider.GetRequiredService<MsSqlServerConnectionService>(),
            _ => throw new NotImplementedException(),
        };
    }
}
