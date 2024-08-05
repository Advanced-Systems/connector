using System;
using System.Data;

using AdvancedSystems.Connector.Abstractions;
using AdvancedSystems.Connector.Options;

using Microsoft.Data.SqlClient;

using DatabaseCommandType = AdvancedSystems.Connector.Abstractions.DatabaseCommandType;
using InternalCommandType = System.Data.CommandType;

namespace AdvancedSystems.Connector.Converters;

internal static class MsSqlServerExtensions
{
    #region Helpers

    internal static string CreateConnectionString(this MsSqlServerSettings settings)
    {
        var builder = new SqlConnectionStringBuilder
        {
            UserID = settings.UserID,
            Password = settings.Password,
            DataSource = settings.DataSource,
            InitialCatalog = settings.InitialCatalog,
            MinPoolSize = settings.MinPoolSize,
            MaxPoolSize = settings.MaxPoolSize,
            ConnectTimeout = settings.ConnectTimeout,
            CommandTimeout = settings.CommandTimeout,
            TrustServerCertificate = settings.TrustServerCertificate,
            ApplicationName = settings.ApplicationName,
            Encrypt = settings.Encrypt,
        };

        return builder.ToString();
    }

    #endregion

    #region Converters

    internal static InternalCommandType Cast(this DatabaseCommandType commandType)
    {
        return commandType switch
        {
            DatabaseCommandType.Text => InternalCommandType.Text,
            DatabaseCommandType.StoredProcedure => InternalCommandType.StoredProcedure,
            _ => throw new NotImplementedException(Enum.GetName(commandType)),
        };
    }

    internal static SqlParameter Cast(this IDatabaseParameter parameter)
    {
        return new SqlParameter
        {
            ParameterName = parameter.ParameterName,
            SqlDbType = parameter.SqlDbType,
            Value = parameter.Value,
        };
    }

    internal static SqlDbType Cast(this Type type)
    {
        var typeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(type) ?? type);

        return typeCode switch
        {
            TypeCode.String => SqlDbType.VarChar,
            TypeCode.Char => SqlDbType.Char,
            TypeCode.Byte => SqlDbType.TinyInt,
            TypeCode.SByte => SqlDbType.SmallInt,
            TypeCode.Int32 => SqlDbType.Int,
            TypeCode.Int64 => SqlDbType.BigInt,
            TypeCode.Single or TypeCode.Double => SqlDbType.Real,
            TypeCode.Decimal=> SqlDbType.Decimal,
            TypeCode.Boolean => SqlDbType.Bit,
            TypeCode.DateTime => SqlDbType.DateTime,
            _ => throw new ArgumentException($"Failed to infer type from {typeCode}."),
        };
    }

    #endregion
}
