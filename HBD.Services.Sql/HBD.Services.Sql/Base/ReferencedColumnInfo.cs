namespace HBD.Services.Sql.Base
{
    public class ReferencedColumnInfo
    {
        #region Constructors

        public ReferencedColumnInfo(string referencedTableSchema, string referencedTableName, string referencedColumn)
            : this(new DbName(referencedTableSchema, referencedTableName), referencedColumn)
        {
        }

        public ReferencedColumnInfo(DbName referencedTable, string referencedColumn)
        {
            ReferencedTable = referencedTable;
            ReferencedColumn = referencedColumn;
        }

        #endregion Constructors

        #region Properties

        public string ReferencedColumn { get; }

        public DbName ReferencedTable { get; }

        #endregion Properties
    }
}