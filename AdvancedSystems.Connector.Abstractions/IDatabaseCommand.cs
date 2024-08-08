using System.Collections.Generic;

namespace AdvancedSystems.Connector.Abstractions;

public interface IDatabaseCommand
{
    #region Properties

    string CommandText { get; set; }

    DatabaseCommandType CommandType { get; set; }

    List<IDatabaseParameter> Parameters { get; set; }

    #endregion

    #region Methods

    void AddParameter(IDatabaseParameter parameter);

    void AddParameter<T>(string name, T value);

    #endregion
}
