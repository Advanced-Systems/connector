using System.Data;

namespace AdvancedSystems.Connector.Abstractions;

public interface IDbParameter
{
    #region Properties

    string ParameterName { get; set; }

    SqlDbType SqlDbType { get; set; }

    object? Value { get; set; }

    #endregion
}
