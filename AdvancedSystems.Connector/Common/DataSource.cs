namespace AdvancedSystems.Connector.Common;

public sealed class DataSource
{
    public DataSource(string host, int port)
    {
        this.Host = host;
        this.Port = port;
    }

    #region Properties

    public string Host { get; set; }

    public int Port { get; set; }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{this.Host},{this.Port}";
    }

    #endregion

    #region Operators

    public static implicit operator string(DataSource dataSource)
    {
        return dataSource.ToString();
    }

    #endregion
}
