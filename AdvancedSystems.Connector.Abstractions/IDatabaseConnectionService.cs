using System.Data;

namespace AdvancedSystems.Connector.Abstractions;

public interface IDatabaseConnectionService
{
    #region Properties

    string ConnectionString { get; }

    #endregion

    #region Methods

    DataSet ExecuteQuery(IDatabaseCommand command);

    int ExecuteNonQuery(IDatabaseCommand command);

    #endregion
}
