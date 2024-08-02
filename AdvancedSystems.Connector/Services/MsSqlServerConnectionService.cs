using System;

using AdvancedSystems.Connector.Abstractions;
using AdvancedSystems.Connector.Options;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Connector.Services;

public sealed class MsSqlServerConnectionService : IDbConnectionService
{
    private readonly ILogger<MsSqlServerConnectionService> _logger;
    private readonly MsSqlServerSettings _settings;

    public MsSqlServerConnectionService(ILogger<MsSqlServerConnectionService> logger, IOptions<MsSqlServerSettings> options)
    {
        this._logger = logger;
        this._settings = options.Value;

        this.ConnectionString = this.CreateConnectionString();
    }

    private string CreateConnectionString()
    {
        var builder = new SqlConnectionStringBuilder
        {
            UserID = this._settings.UserID,
            Password = this._settings.Password,
            DataSource = this._settings.DataSource,
            InitialCatalog = this._settings.InitialCatalog,
            MinPoolSize = this._settings.MinPoolSize,
            MaxPoolSize = this._settings.MaxPoolSize,
            ConnectTimeout = this._settings.ConnectTimeout,
            CommandTimeout = this._settings.CommandTimeout,
            TrustServerCertificate = this._settings.TrustServerCertificate,
            ApplicationName = this._settings.ApplicationName,
            Encrypt = this._settings.Encrypt,
        };

        return builder.ConnectionString;
    }

    #region Properties

    public string ConnectionString { get; private set; }

    #endregion

    #region Methods

    public void ExecuteQuery()
    {
        throw new NotImplementedException();
    }

    public void ExecuteNonQuery()
    {
        throw new NotImplementedException();
    }

    #endregion
}
