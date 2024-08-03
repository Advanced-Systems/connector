using System;

namespace AdvancedSystems.Connector.Abstractions.Exceptions;

public class DbCommandExecutionException : Exception
{
    public DbCommandExecutionException()
    {

    }

    public DbCommandExecutionException(string message) : base(message)
    {

    }

    public DbCommandExecutionException(string message, Exception inner) : base(message, inner)
    {

    }
}
