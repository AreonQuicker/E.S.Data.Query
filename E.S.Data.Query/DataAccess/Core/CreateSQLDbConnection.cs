using System.Data;
using System.Data.SqlClient;
using E.S.Data.Query.DataAccess.Interfaces;

namespace E.S.Data.Query.DataAccess.Core
{
    internal class CreateSQLDbConnection : ICreateDbConnection
    {
        #region Private Read Only Fields

        #endregion

        #region Constructor

        public CreateSQLDbConnection(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region ICreateDbConnection

        public string ConnectionString { get; }

        IDbConnection ICreateDbConnection.CreateDbConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        IDbConnection ICreateDbConnection.CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        #endregion
    }
}