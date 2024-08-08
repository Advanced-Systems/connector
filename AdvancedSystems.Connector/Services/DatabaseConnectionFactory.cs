using System;

using AdvancedSystems.Connector.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace AdvancedSystems.Connector.Services;

public sealed class DatabaseConnectionFactory : IDatabaseConnectionFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseConnectionFactory(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public IDatabaseConnectionService Create(Provider provider)
    {
        return provider switch
        {
            Provider.MsSql => this._serviceProvider.GetRequiredService<MsSqlServerConnectionService>(),
            _ => throw new NotImplementedException(Enum.GetName(provider)),
        };
    }
}
