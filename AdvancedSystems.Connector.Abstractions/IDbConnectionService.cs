namespace AdvancedSystems.Connector.Abstractions;

public interface IDbConnectionService
{
    #region Properties

    string ConnectionString { get; }

    #endregion

    #region Methods

    void ExecuteQuery();

    void ExecuteNonQuery();

    #endregion
}
