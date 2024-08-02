namespace AdvancedSystems.Connector.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnectionService Create(Provider provider);
}
