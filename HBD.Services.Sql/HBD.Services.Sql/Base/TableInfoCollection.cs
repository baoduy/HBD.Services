using System.Collections.Generic;

namespace HBD.Services.Sql.Base
{
    public class TableInfoCollection : DbInfoCollection<TableInfo>
    {
        #region Constructors

        internal TableInfoCollection(SchemaInfo parentSchema) : base(parentSchema, null)
        {
        }

        internal TableInfoCollection(SchemaInfo parentSchema, IEnumerable<TableInfo> collection)
            : base(parentSchema, collection)
        {
        }

        #endregion Constructors
    }
}