using System;
using System.Collections.Generic;

using AdvancedSystems.Connector.Abstractions;
using AdvancedSystems.Connector.Converters;

namespace AdvancedSystems.Connector.Common;

public sealed class DbCommand : IDbCommand
{
    #region Properties

    public required string CommandText {  get; set; }

    public required CommandType CommandType { get; set; }

    public required List<IDbParameter> Parameters { get; set; }

    #endregion

    #region Methods

    public void AddParameter(IDbParameter parameter)
    {
        this.Parameters.Add(parameter);
    }

    public void AddParameter<T>(string name, T value)
    {
        var parameter = new DbParameter
        {
            ParameterName = name,
            SqlDbType = typeof(T).Cast(),
            Value = (object?)value ?? DBNull.Value
        };

        this.Parameters.Add(parameter);
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    #endregion
}
