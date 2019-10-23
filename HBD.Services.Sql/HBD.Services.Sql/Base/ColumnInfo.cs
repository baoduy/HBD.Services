using System.Data;
using System.Diagnostics;
using System.Linq;

namespace HBD.Services.Sql.Base
{
    [DebuggerDisplay("Column Name = {Name}, IsPrimaryKey = {IsPrimaryKey}")]
    public class ColumnInfo
    {
        #region Fields

        private bool _isNullable = true;

        #endregion Fields

        #region Properties

        public string ComputedExpression { get; set; }

        public SqlDbType DataType { get; set; }

        public bool IsComputed { get; set; }

        public bool IsForeignKey => Table.ForeignKeys.Any(f => f.Column == this);

        public bool IsIdentity { get; set; }

        public bool IsNullable
        {
            get => !IsPrimaryKey && !IsComputed && _isNullable;
            set => _isNullable = value;
        }

        public bool IsPrimaryKey { get; set; }

        public int MaxLengh { get; set; }

        public object MaxPrimaryKeyValue { get; set; }

        public string Name { get; set; }

        public int OrdinalPosition { get; set; }

        public TableInfo Table { get; protected internal set; }

        #endregion Properties

        #region Methods

        public override string ToString() => Name;

        #endregion Methods
    }
}