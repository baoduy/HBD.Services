#region using

using System.Diagnostics;

#endregion

namespace HBD.Services.Sql.Base
{
    [DebuggerDisplay("Schema Name = {" + nameof(Name) + "}")]
    public class DatabaseInfo
    {
        private readonly ShemaInfoService _sqlClient;

        protected internal DatabaseInfo(ShemaInfoService sqlClient, string name)
        {
            _sqlClient = sqlClient;
            Name = name;
        }

        public string Name { get; }

        public SchemaInfo GetSchemaInfo() => _sqlClient?.GetSchemaInfo(Name);
    }
}