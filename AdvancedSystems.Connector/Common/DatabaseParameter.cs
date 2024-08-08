using System.Data;

using AdvancedSystems.Connector.Abstractions;

namespace AdvancedSystems.Connector.Common;

public sealed class DatabaseParameter : IDatabaseParameter
{
    #region Properties

    public required string ParameterName { get; set; }

    public required SqlDbType SqlDbType { get; set; }

    public required object? Value { get; set; }

    #endregion
}
