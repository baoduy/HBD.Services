using System.Diagnostics;

namespace HBD.Services.Sql.Base
{
    [DebuggerDisplay("Schema Name = {Name}, Tables= {Tables.Count}")]
    public class SchemaInfo
    {
        #region Constructors

        protected internal SchemaInfo(string name)
        {
            Name = name;
            Tables = new TableInfoCollection(this);
            Views = new ViewInfoCollection(this);
        }

        #endregion Constructors

        #region Properties

        public string Name { get; set; }

        public TableInfoCollection Tables { get; protected internal set; }

        public ViewInfoCollection Views { get; protected internal set; }

        #endregion Properties
    }
}