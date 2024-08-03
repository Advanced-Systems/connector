namespace AdvancedSystems.Connector.Abstractions;

/// <summary>
///     Specifies how a command string is interpreted.
/// </summary>
public enum CommandType
{
    /// <summary>
    ///     An SQL command.
    /// </summary>
    Text,
    /// <summary>
    ///     The name of a stored procedure.
    /// </summary>
    StoredProcedure
}
