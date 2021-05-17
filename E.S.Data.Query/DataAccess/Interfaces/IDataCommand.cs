using System;
using System.Data;

namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataCommand : IDataAccessQuery, IDisposable
    {
        IDbConnection DbConnection { get; }
        void BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted);
        void Commit();
        void Dispose();
        void Rollback();
        void CloseConnection();

        void OpenConnection();
    }
}