using System;
using System.Data;
using System.Data.Common;
using System.Linq;

using AdvancedSystems.Connector.Abstractions;
using AdvancedSystems.Connector.Abstractions.Exceptions;
using AdvancedSystems.Connector.Converters;
using AdvancedSystems.Connector.Options;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using IDbCommand = AdvancedSystems.Connector.Abstractions.IDbCommand;

namespace AdvancedSystems.Connector.Services;

public sealed class MsSqlServerConnectionService : IDbConnectionService
{
    private readonly ILogger<MsSqlServerConnectionService> _logger;
    private readonly MsSqlServerSettings _settings;

    public MsSqlServerConnectionService(ILogger<MsSqlServerConnectionService> logger, IOptions<MsSqlServerSettings> options)
    {
        this._logger = logger;
        this._settings = options.Value;

        this.ConnectionString = this._settings.CreateConnectionString();
    }

    #region Properties

    public string ConnectionString { get; private set; }

    #endregion

    #region Methods

    public DataSet ExecuteQuery(IDbCommand command)
    {
        DataSet dataSet = new();
        using var connection = new SqlConnection(this.ConnectionString);
        using var sqlCommand = connection.CreateCommand();

        try
        {
            sqlCommand.CommandText = command.CommandText;
            sqlCommand.CommandType = command.CommandType.Cast();
            sqlCommand.Parameters.AddRange(command.Parameters.Select(p => p.Cast()).ToArray());

            using var reader = sqlCommand.ExecuteReader();
            using var adapter = new SqlDataAdapter(sqlCommand);
            adapter.Fill(dataSet);
            return dataSet;
        }
        catch (SqlException exception)
        {
            throw new DbCommandExecutionException($"Database failed to execute command {sqlCommand.CommandText} ({exception.Message}).", exception);
        }
        catch (DbException exception)
        {
            throw new DbConnectionException($"Communication to database failed during the execution of {sqlCommand.CommandText} ({exception.Message}).", exception);
        }
    }

    public int ExecuteNonQuery(IDbCommand command)
    {
        throw new NotImplementedException();
    }

    #endregion
}
