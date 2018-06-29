#region using

using System.Collections.Generic;

#endregion

namespace HBD.Services.Sql.Base
{
    public class ViewInfoCollection : DbInfoCollection<ViewInfo>
    {
        internal ViewInfoCollection(SchemaInfo parentSchema) : base(parentSchema, null)
        {
        }

        internal ViewInfoCollection(SchemaInfo parentSchema, IEnumerable<ViewInfo> collection)
            : base(parentSchema, collection)
        {
        }
    }
}