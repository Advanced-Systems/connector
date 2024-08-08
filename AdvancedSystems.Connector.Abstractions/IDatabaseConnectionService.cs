using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace AdvancedSystems.Connector.Abstractions;

public interface IDatabaseConnectionService
{
    #region Properties

    string ConnectionString { get; }

    #endregion

    #region Methods

    DataSet ExecuteQuery(IDatabaseCommand databaseCommand);

    ValueTask<DataSet?> ExecuteQueryAsync(IDatabaseCommand databaseCommand, CancellationToken cancellationToken);

    int ExecuteNonQuery(IDatabaseCommand databaseCommand);

    ValueTask<int> ExecuteNonQueryAsync(IDatabaseCommand databaseCommand, CancellationToken cancellationToken);

    #endregion
}
