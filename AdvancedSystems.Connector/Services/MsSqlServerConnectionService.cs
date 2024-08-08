using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Connector.Abstractions;
using AdvancedSystems.Connector.Abstractions.Exceptions;
using AdvancedSystems.Connector.Converters;
using AdvancedSystems.Connector.Options;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdvancedSystems.Connector.Services;

public sealed class MsSqlServerConnectionService : IDatabaseConnectionService
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

    public ConnectionState ConnectionState { get; private set; }

    #endregion

    #region Helpers

    private void ConnectionStateHandler(object sender, StateChangeEventArgs e)
    {
        this.ConnectionState = e.CurrentState;
    }

    private void InfoMessageHandler(object sender, SqlInfoMessageEventArgs e)
    {
        if (e.Errors.Count == 0) return;

        byte warningThreshold = 10;
        byte errorThreshold = 20;
        SqlError lastError = e.Errors[^1];

        if (lastError.Class <= warningThreshold)
        {
            this._logger.LogWarning("{Server} issued the following warning: {Message}.", lastError.Server, lastError.Message);
        }
        else if (lastError.Class > warningThreshold && lastError.Class <= errorThreshold)
        {
            this._logger.LogError("{Server} raised an error on line {Line}: {Message} (State={State}).", lastError.Server, lastError.LineNumber, lastError.Message, lastError.State);
        }
        else
        {
            // A severity over 20 causes the connection to close
            string reason = "Connection was closed";
            this._logger.LogCritical("{Reason} ({Message}).", reason, lastError.Message);
            throw new DbConnectionException($"{reason} ({lastError.Message}).");
        }
    }

    #endregion

    #region Methods

    public DataSet ExecuteQuery(IDatabaseCommand databaseCommand)
    {
        DataSet result = new();

        try
        {
            using var connection = new SqlConnection(this.ConnectionString);
            connection.StateChange += ConnectionStateHandler;
            connection.InfoMessage += InfoMessageHandler;
            connection.Open();

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = databaseCommand.CommandText;
            sqlCommand.CommandType = databaseCommand.CommandType.Cast();

            if (!databaseCommand.Parameters.IsNullOrEmpty())
            {
                var parameters = databaseCommand.Parameters.Select(p => p.Cast()).ToArray();
                sqlCommand.Parameters.AddRange(parameters);
            }

            using var adapter = new SqlDataAdapter(sqlCommand);
            adapter.Fill(result);
            this._logger.LogTrace("Executed query '{Query}'.", databaseCommand);
        }
        catch (SqlException exception)
        {
            string reason = $"Database failed to execute command {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbCommandExecutionException($"{reason} ({exception.Message}).", exception);
        }
        catch (DbException exception)
        {
            string reason = $"Communication to database failed during the execution of {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbConnectionException($"{reason} ({exception.Message}).", exception);
        }

        return result;
    }

    public async ValueTask<DataSet?> ExecuteQueryAsync(IDatabaseCommand databaseCommand, CancellationToken cancellationToken)
    {
        DataTable result = new();

        try
        {
            using var connection = new SqlConnection(this.ConnectionString);
            connection.StateChange += ConnectionStateHandler;
            connection.InfoMessage += InfoMessageHandler;
            await connection.OpenAsync(cancellationToken);

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = databaseCommand.CommandText;
            sqlCommand.CommandType = databaseCommand.CommandType.Cast();

            if (!databaseCommand.Parameters.IsNullOrEmpty())
            {
                var parameters = databaseCommand.Parameters.Select(p => p.Cast()).ToArray();
                sqlCommand.Parameters.AddRange(parameters);
            }

            using var reader = await sqlCommand.ExecuteReaderAsync(cancellationToken);
            result.Load(reader);
            this._logger.LogTrace("Executed query '{Query}'.", databaseCommand);
        }
        catch (SqlException exception)
        {
            string reason = $"Database failed to execute command {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbCommandExecutionException($"{reason} ({exception.Message}).", exception);
        }
        catch (DbException exception)
        {
            string reason = $"Communication to database failed during the execution of {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbConnectionException($"{reason} ({exception.Message}).", exception);
        }

        return result.DataSet;
    }

    public int ExecuteNonQuery(IDatabaseCommand databaseCommand)
    {
        int rowsAffected = default;

        try
        {
            using var connection = new SqlConnection(this.ConnectionString);
            connection.StateChange += ConnectionStateHandler;
            connection.InfoMessage += InfoMessageHandler;
            connection.Open();

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = databaseCommand.CommandText;
            sqlCommand.CommandType = databaseCommand.CommandType.Cast();

            if (!databaseCommand.Parameters.IsNullOrEmpty())
            {
                var parameters = databaseCommand.Parameters.Select(p => p.Cast()).ToArray();
                sqlCommand.Parameters.AddRange(parameters);
            }

            rowsAffected = sqlCommand.ExecuteNonQuery();
            this._logger.LogTrace("Executed query '{Query}'.", databaseCommand);
        }
        catch (SqlException exception)
        {
            string reason = $"Database failed to execute command {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbCommandExecutionException($"{reason} ({exception.Message}).", exception);
        }
        catch (DbException exception)
        {
            string reason = $"Communication to database failed during the execution of {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbConnectionException($"{reason} ({exception.Message}).", exception);
        }

        return rowsAffected;
    }

    public async ValueTask<int> ExecuteNonQueryAsync(IDatabaseCommand databaseCommand, CancellationToken cancellationToken)
    {
        int rowsAffected = default;

        try
        {
            using var connection = new SqlConnection(this.ConnectionString);
            connection.StateChange += ConnectionStateHandler;
            connection.InfoMessage += InfoMessageHandler;
            await connection.OpenAsync(cancellationToken);

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = databaseCommand.CommandText;
            sqlCommand.CommandType = databaseCommand.CommandType.Cast();

            if (!databaseCommand.Parameters.IsNullOrEmpty())
            {
                var parameters = databaseCommand.Parameters.Select(p => p.Cast()).ToArray();
                sqlCommand.Parameters.AddRange(parameters);
            }

            rowsAffected = await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
            this._logger.LogTrace("Executed query '{Query}'.", databaseCommand);
        }
        catch (SqlException exception)
        {
            string reason = $"Database failed to execute command {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbCommandExecutionException($"{reason} ({exception.Message}).", exception);
        }
        catch (DbException exception)
        {
            string reason = $"Communication to database failed during the execution of {databaseCommand}";
            this._logger.LogError("{Reason} ({Message}).", reason, exception.Message);
            throw new DbConnectionException($"{reason} ({exception.Message}).", exception);
        }

        return rowsAffected;
    }

    #endregion
}
