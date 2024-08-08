using System.Data;

namespace AdvancedSystems.Connector.Abstractions;

public interface IDatabaseParameter
{
    #region Properties

    string ParameterName { get; set; }

    SqlDbType SqlDbType { get; set; }

    object? Value { get; set; }

    #endregion
}
