namespace HBD.Services.Sql.Base
{
    public interface IDbInfo
    {
        DbName Name { get; }
        SchemaInfo Schema { get; set; }
    }
}