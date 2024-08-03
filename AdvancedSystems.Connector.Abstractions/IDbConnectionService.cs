using System.Data;

namespace AdvancedSystems.Connector.Abstractions;

public interface IDbConnectionService
{
    #region Properties

    string ConnectionString { get; }

    #endregion

    #region Methods

    DataSet ExecuteQuery(IDbCommand command);

    int ExecuteNonQuery(IDbCommand command);

    #endregion
}
