using E.S.Data.Query.DataAccess.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace E.S.Data.Query.DataAccess.Core
{

    public class CreateSQLDbConnection : ICreateDbConnection
    {
        #region Private Read Only Fields
        private readonly string connectionString;
        #endregion

        #region Constructor
        public CreateSQLDbConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }
        #endregion

        #region ICreateDbConnection
        public string ConnectionString => connectionString;

        IDbConnection ICreateDbConnection.CreateDbConnection()
        {
            return new SqlConnection(connectionString);
        }

        IDbConnection ICreateDbConnection.CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        #endregion
    }
}
