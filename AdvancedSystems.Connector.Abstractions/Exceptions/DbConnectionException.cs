using System;

namespace AdvancedSystems.Connector.Abstractions.Exceptions;

public class DbConnectionException : Exception
{
    public DbConnectionException()
    {

    }

    public DbConnectionException(string message) : base(message)
    {

    }

    public DbConnectionException(string message, Exception inner) : base(message, inner)
    {

    }
}
