namespace AdvancedSystems.Connector.Abstractions;

public interface IDatabaseConnectionFactory
{
    IDatabaseConnectionService Create(Provider provider);
}
