using System.Data;

namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface ICreateDbConnection
    {
        string ConnectionString { get; }

        IDbConnection CreateDbConnection();

        IDbConnection CreateDbConnection(string connectionString);
    }
}
