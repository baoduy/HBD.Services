using HBD.Framework.Collections;
using System.Collections.Generic;

namespace HBD.Services.Sql.Base
{
    public class DbInfoCollection<TDbInfo> : DistinctCollection<DbName, TDbInfo> where TDbInfo : IDbInfo
    {
        #region Constructors

        public DbInfoCollection(SchemaInfo parentSchema) : this(parentSchema, null)
        {
        }

        public DbInfoCollection(SchemaInfo parentSchema, IEnumerable<TDbInfo> collection) : base(t => t.Name)
        {
            ParentSchema = parentSchema;
            AddRange(collection);
        }

        #endregion Constructors

        #region Properties

        private SchemaInfo ParentSchema { get; }

        #endregion Properties

        #region Methods

        public new void Add(TDbInfo item)
        {
            item.Schema = ParentSchema;
            base.Add(item);
        }

        public void AddRange(IEnumerable<TDbInfo> collection)
        {
            if (collection == null) return;
            foreach (var c in collection) Add(c);
        }

        #endregion Methods
    }
}