using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

using AdvancedSystems.Connector.Abstractions;
using AdvancedSystems.Connector.Converters;

namespace AdvancedSystems.Connector.Common;

[DebuggerDisplay("{CommandText}")]
public sealed class DatabaseCommand : IDatabaseCommand
{
    #region Properties

    [DisplayName("Command Text")]
    public required string CommandText {  get; set; }

    [DisplayName("Command Type")]
    public required DatabaseCommandType CommandType { get; set; }

    [DisplayName("Parameters")]
    public List<IDatabaseParameter> Parameters { get; set; } = [];

    #endregion

    #region Methods

    public void AddParameter(IDatabaseParameter parameter)
    {
        this.Parameters.Add(parameter);
    }

    public void AddParameter<T>(string name, T value)
    {
        var parameter = new DatabaseParameter
        {
            ParameterName = name,
            SqlDbType = typeof(T).Cast(),
            Value = (object?)value ?? DBNull.Value
        };

        this.Parameters.Add(parameter);
    }

    private static string? FormatValue(IDatabaseParameter parameter)
    {
        // TODO: Make implementation more robust and account for different output formats
        return parameter.Value?.ToString();
    }

    public override string ToString()
    {
        if (this.Parameters.Count == 0) return this.CommandText;

        var commandBuilder = new StringBuilder(this.CommandText);

        foreach (var parameter in this.Parameters)
        {
            commandBuilder.Replace(parameter.ParameterName, FormatValue(parameter));
        }

        return commandBuilder.ToString();
    }

    #endregion
}
