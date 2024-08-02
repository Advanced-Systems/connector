using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.Data.SqlClient;

namespace AdvancedSystems.Connector.Options;

public class DbSettings
{
    /// <summary>
    ///     Gets or sets the name or network address of the instance of SQL Server to connect to.
    /// </summary>
    [Required]
    [DisplayName("Data Source")]
    public required string DataSource { get; set; }

    /// <summary>
    ///     Gets or sets the name of the database associated with the connection.
    /// </summary>
    [Required]
    [DisplayName("Initial Catalog")]
    public required string InitialCatalog { get; set; }

    /// <summary>
    ///     Gets or sets the password for the SQL Server account.
    /// </summary>
    [Required]
    [DisplayName("Password")]
    public required string Password { get; set; }

    /// <summary>
    ///     Gets or sets the user ID to be used when connecting to SQL Server.
    /// </summary>
    [Required]
    [DisplayName("User ID")]
    public required string UserID { get; set; }
}
