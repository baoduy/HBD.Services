using HBD.Framework.Core;

namespace HBD.Services.Sql.Base
{
    public class ForeignKeyInfo
    {
        #region Constructors

        public ForeignKeyInfo(string name, ColumnInfo column, ReferencedColumnInfo referencedColumn)
        {
            Guard.ArgumentIsNotNull(referencedColumn, nameof(referencedColumn));
            Column = column;
            ReferencedColumn = referencedColumn;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public ColumnInfo Column { get; }

        public string Name { get; }

        public ReferencedColumnInfo ReferencedColumn { get; }

        #endregion Properties
    }
}