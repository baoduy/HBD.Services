namespace HBD.Services.Sql.Base
{
    public interface IDbInfo
    {
        #region Properties

        DbName Name { get; }

        SchemaInfo Schema { get; set; }

        #endregion Properties
    }
}