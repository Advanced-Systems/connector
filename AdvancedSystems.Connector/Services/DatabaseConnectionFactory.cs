using System;

using AdvancedSystems.Connector.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Connector.Services;

public sealed class DatabaseConnectionFactory : IDatabaseConnectionFactory
{
    private readonly ILogger<DatabaseConnectionFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DatabaseConnectionFactory(ILogger<DatabaseConnectionFactory> logger, IServiceProvider serviceProvider)
    {
        this._logger = logger;
        this._serviceProvider = serviceProvider;
    }

    public IDatabaseConnectionService Create(Provider provider)
    {
        return provider switch
        {
            Provider.MsSql => this._serviceProvider.GetRequiredService<MsSqlServerConnectionService>(),
            _ => throw new NotImplementedException(),
        };
    }
}
