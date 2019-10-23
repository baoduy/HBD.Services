using System.Diagnostics;

namespace HBD.Services.Sql.Base
{
    [DebuggerDisplay("Schema Name = {" + nameof(Name) + "}")]
    public class DatabaseInfo
    {
        #region Fields

        private readonly ShemaInfoService _sqlClient;

        #endregion Fields

        #region Constructors

        protected internal DatabaseInfo(ShemaInfoService sqlClient, string name)
        {
            _sqlClient = sqlClient;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public string Name { get; }

        #endregion Properties

        #region Methods

        public SchemaInfo GetSchemaInfo() => _sqlClient?.GetSchemaInfo(Name);

        #endregion Methods
    }
}