using System.Collections.Generic;

namespace HBD.Services.Sql.Base
{
    public class ViewInfoCollection : DbInfoCollection<ViewInfo>
    {
        #region Constructors

        internal ViewInfoCollection(SchemaInfo parentSchema) : base(parentSchema, null)
        {
        }

        internal ViewInfoCollection(SchemaInfo parentSchema, IEnumerable<ViewInfo> collection)
            : base(parentSchema, collection)
        {
        }

        #endregion Constructors
    }
}