using System.Collections.Generic;

namespace AdvancedSystems.Connector.Abstractions;

public interface IDbCommand
{
    #region Properties

    string CommandText { get; set; }

    CommandType CommandType { get; set; }

    List<IDbParameter> Parameters { get; set; }

    #endregion

    #region Methods

    void AddParameter(IDbParameter parameter);

    void AddParameter<T>(string name, T value);

    #endregion
}
