using HBD.Framework.Collections;
using HBD.Framework.Core;
using System.Collections.Generic;

namespace HBD.Services.Sql.Base
{
    public class ColumnInfoCollection : DistinctCollection<string, ColumnInfo>
    {
        #region Constructors

        internal ColumnInfoCollection(TableInfo parentTable) : this(parentTable, null)
        {
        }

        internal ColumnInfoCollection(TableInfo parentTable, IEnumerable<ColumnInfo> collection) : base(c => c.Name)
        {
            ParentTable = parentTable;
            AddRange(collection);
        }

        #endregion Constructors

        #region Properties

        private TableInfo ParentTable { get; }

        #endregion Properties

        #region Methods

        public new void Add(ColumnInfo item)
        {
            Guard.ArgumentIsNotNull(item, nameof(item));
            Guard.ArgumentIsNotNull(item.Name, "ColumnName");

            item.Table = ParentTable;
            base.Add(item);
        }

        public void AddRange(IEnumerable<ColumnInfo> collection)
        {
            if (collection == null) return;
            foreach (var c in collection) Add(c);
        }

        #endregion Methods
    }
}