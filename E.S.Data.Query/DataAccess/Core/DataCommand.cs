using E.S.Data.Query.DataAccess.Interfaces;
using System;
using System.Data;

namespace E.S.Data.Query.DataAccess.Core
{

    public class DataCommand : DataAccessQuery, IDataAccessQuery, IDisposable, IDataCommand
    {

        #region Constructor

        public DataCommand(ICreateDbConnection createDbConnection, bool keepConnectionClosed = true)
            : base(createDbConnection, keepConnectionClosed)
        {
            dbConnection = createDbConnection.CreateDbConnection();
        }
        #endregion

        #region IDataCommand    
        public IDbConnection DbConnection => dbConnection;

        public void BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            if (dbTransaction != null) return;

            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();

            keepConnectionClosed = false;

            dbTransaction = dbConnection.BeginTransaction(level);
        }

        public void OpenConnection()
        {
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
        }

        public void CloseConnection()
        {
            if (dbConnection.State == System.Data.ConnectionState.Open)
                dbConnection.Close();
        }

        public void Rollback()
        {
            if (dbTransaction == null) return;

            dbTransaction.Rollback();
            dbTransaction.Dispose();
            dbTransaction = null;
        }
        public void Commit()
        {
            if (dbTransaction == null) return;

            dbTransaction.Commit();
            dbTransaction.Dispose();
            dbTransaction = null;

            CloseConnection();
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbTransaction = null;
            }

            CloseConnection();
        }
        #endregion
    }
}
