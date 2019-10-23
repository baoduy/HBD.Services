using System.Collections.Generic;
using System.Diagnostics;

namespace HBD.Services.Sql.Base
{
    [DebuggerDisplay("Table Name = {Name}, Columns={Columns.Count}")]
    public class ViewInfo : IDbInfo
    {
        #region Constructors

        public ViewInfo(string schema, string name) : this(new DbName(schema, name))
        {
        }

        public ViewInfo(DbName name)
        {
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public IList<string> Columns { get; } = new List<string>();

        public DbName Name { get; }

        public SchemaInfo Schema { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString() => Name;

        #endregion Methods
    }
}