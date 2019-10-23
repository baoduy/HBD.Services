using HBD.Framework.Core;
using HBD.Framework.Data;
using HBD.Framework.Data.Base;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HBD.Services.Sql
{
    public class SqlClientAdapter : DataClientAdapter
    {
        #region Constructors

        public SqlClientAdapter(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public SqlClientAdapter(DbConnectionStringBuilder connectionString) : base(connectionString)
        {
        }

        public SqlClientAdapter(IDbConnection connection) : base(connection)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Insert DataTable to SQL using SqlBulkCopy. DataTable should have the name according with
        ///     SQL Table Name.
        /// </summary>
        /// <param name="data">DataTable</param>
        public virtual void SqlBulkInsert(DataTable data)
        {
            Guard.ArgumentIsNotNull(data, "DataTable");
            Guard.ArgumentIsNotNull(data.TableName, "TableName");

            using (var sbc = CreateSqlBulkCopy())
            {
                try
                {
                    Connection.TryOpen();
                    sbc.DestinationTableName = data.TableName;
                    sbc.WriteToServer(data);
                }
                finally
                {
                    Connection.TryClose();
                }
            }
        }

        protected override IDbConnection CreateConnection() => new SqlConnection(ConnectionString.ConnectionString);

        protected override DbConnectionStringBuilder CreateConnectionString(string connectionString)
            => new SqlConnectionStringBuilder(connectionString);

        protected override IDataParameter CreateParameter(string name, object value) => new SqlParameter(name, value);

        protected virtual SqlBulkCopy CreateSqlBulkCopy() => new SqlBulkCopy((SqlConnection)Connection);

        #endregion Methods
    }
}