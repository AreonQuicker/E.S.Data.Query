using System.Data;
using E.S.Data.Query.DataAccess.Interfaces;

namespace E.S.Data.Query.DataAccess.Core
{
    public class DataCommand : DataAccessQuery, IDataCommand
    {
        #region Fields

        private readonly ICreateDbConnection createDbConnection;

        #endregion

        #region Constructor

        public DataCommand(
            ICreateDbConnection createDbConnection,
            bool newConnectionOnEachProcess = true,
            bool keepConnectionClosed = true)
            : base(createDbConnection, newConnectionOnEachProcess, keepConnectionClosed)
        {
            this.createDbConnection = createDbConnection;
        }

        #endregion

        #region IDisposable

        public new void Dispose()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbTransaction = null;
            }

            base.Dispose();
        }

        #endregion

        #region Private methods

        private void OpenConnection()
        {
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
        }

        #endregion

        #region IDataCommand

        public IDbConnection DbConnection => dbConnection;

        public void BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            if (dbTransaction != null) return;

            if (DbConnection == null) dbConnection = createDbConnection.CreateDbConnection();

            OpenConnection();

            newConnectionOnEachProcess = false;
            keepConnectionClosed = false;

            dbTransaction = dbConnection.BeginTransaction(level);
        }

        public void Rollback()
        {
            if (dbTransaction == null) return;

            dbTransaction.Rollback();
            dbTransaction.Dispose();
            dbTransaction = null;

            CloseConnection();
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
    }
}