using System.ComponentModel;

using Microsoft.Data.SqlClient;

namespace AdvancedSystems.Connector.Options;

public sealed class MsSqlServerSettings : DbSettings
{
    /// <summary>
    ///     Gets or sets the name of the application associated with the connection string.
    /// </summary>
    [DisplayName("Application Name")]
    public string? ApplicationName { get; set; }

    /// <summary>
    ///     Gets or sets the default wait time (in seconds) before terminating the attempt to execute
    ///     a command and generating an error. The default is 30 seconds.
    /// </summary>
    [DisplayName("Command Timeout")]
    public int CommandTimeout { get; set; }

    /// <summary>
    ///     Gets or sets the length of time (in seconds) to wait for a connection to the server
    ///     before terminating the attempt and generating an error.
    /// </summary>
    [DisplayName("Connect Timeout")]
    public int ConnectTimeout { get; set; }

    /// <summary>
    ///     Gets or sets a <seealso cref="SqlConnectionEncryptOption"/> value that indicates whether
    ///     TLS encryption is required for all data sent between the client and server.
    /// </summary>
    [DisplayName("Encrypt")]
    public SqlConnectionEncryptOption? Encrypt { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of connections allowed in the connection pool for this specific
    ///     connection string.
    /// </summary>
    [DisplayName("Max Pool Size")]
    public int MaxPoolSize { get; set; }

    /// <summary>
    ///     Gets or sets the minimum number of connections allowed in the connection pool for this specific
    ///     connection string.
    /// </summary>
    [DisplayName("Min Pool Size")]
    public int MinPoolSize { get; set; }

    /// <summary>
    ///     Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly
    ///     opened every time that the connection is requested.
    /// </summary>
    [DisplayName("Pooling")]
    public bool Pooling { get; set; }

    /// <summary>
    ///     Gets or sets a value that indicates whether the channel will be encrypted while bypassing
    ///     walking the certificate chain to validate trust.
    /// </summary>
    [DisplayName("Trust Server Certificate")]
    public bool TrustServerCertificate { get; set; }
}
