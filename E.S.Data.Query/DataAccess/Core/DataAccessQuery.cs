using Dapper;
using E.S.Common.Helpers.Extensions;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static E.S.Data.Query.DataQuery.Core.DataQueryInstance;

namespace E.S.Data.Query.DataAccess.Core
{
    public class DataAccessQuery : IDataAccessQuery
    {

        #region Fields   
        protected IDbConnection dbConnection;
        protected IDbTransaction dbTransaction;
        protected bool newConnectionOnEachProcess;
        protected bool keepConnectionClosed;
        private readonly ICreateDbConnection createDbConnection;
        #endregion

        #region Constructor  
        public DataAccessQuery(
            ICreateDbConnection createDbConnection,
            bool newConnectionOnEachProcess = true,
            bool keepConnectionClosed = true)
        {
            dbTransaction = null;
            this.createDbConnection = createDbConnection;
            this.newConnectionOnEachProcess = newConnectionOnEachProcess;
            this.keepConnectionClosed = keepConnectionClosed;

            if (!this.newConnectionOnEachProcess)
            {
                dbConnection = createDbConnection.CreateDbConnection();
            }
            else
            {
                this.keepConnectionClosed = true;
            }
        }
        #endregion

        #region IDataAccessQuery

        public bool NewConnectionOnEachProcess => newConnectionOnEachProcess;

        public (bool IsNew,IDbConnection Connection) NewQueryConnection()
        {
            if (dbConnection != null)
                return (false, dbConnection);

            return (true, createDbConnection.CreateDbConnection());
        }

        public IEnumerable<T> List<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T> result = QueryConnection.Connection.Query<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public IEnumerable<T> List<T>(string procedureName, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T> result = QueryConnection.Connection.Query<T>(
                procedureName,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T> result = await QueryConnection.Connection.QueryAsync<T>(
                procedureName,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T> result = QueryConnection.Connection.Query<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<IEnumerable<T>> ListAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T> result = await QueryConnection.Connection.QueryAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T> result = await QueryConnection.Connection.QueryAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2, P>(string procedureName, P param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T1> First;
            IEnumerable<T2> Second;

            using (SqlMapper.GridReader multi = QueryConnection.Connection.QueryMultiple(procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: CommandType.StoredProcedure))
            {
                First = multi.Read<T1>().ToList();
                Second = multi.Read<T2>().ToList();
            }

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return (First, Second);

        }

        public T First<T, P>(string procedureName, P param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = QueryConnection.Connection.QueryFirst<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public T FirstOrDefault<T, P>(string procedureName, P param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = QueryConnection.Connection.QueryFirstOrDefault<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<T> FirstAsync<T, P>(string procedureName, P param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = await QueryConnection.Connection.QueryFirstAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<T> FirstOrDefaultAsync<T, P>(string procedureName, P param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = await QueryConnection.Connection.QueryFirstOrDefaultAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public T First<T>(string procedureName, DynamicParameters param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = QueryConnection.Connection.QueryFirst<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public T ExecuteScalar<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = QueryConnection.Connection.ExecuteScalar<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<T> ExecuteScalarAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = await QueryConnection.Connection.ExecuteScalarAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public int Execute<P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = QueryConnection.Connection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public int Execute(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = QueryConnection.Connection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ExecuteAsync<P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ExecuteAsync(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public int ExecuteMultiple<P>(string procedureName, params P[] param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = QueryConnection.Connection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public int ExecuteMultiple(string procedureName, List<DynamicParameters> param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = QueryConnection.Connection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<int> ExecuteMultipleAsync<P>(string procedureName, params P[] param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public async Task<int> ExecuteMultipleAsync(string procedureName, List<DynamicParameters> param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;

        }

        public int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            DynamicParameters param = new DynamicParameters();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
            {
                param.AddDynamicParams(extraParam);
            }

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = QueryConnection.Connection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public int Import<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            DynamicParameters param = new DynamicParameters();

            DataTable dt = list.ToDataTableAdvance();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
            {
                param.AddDynamicParams(extraParam);
            }

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = QueryConnection.Connection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            if (extraParam == null)
            {
                extraParam = new DynamicParameters();
            }

            extraParam.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = QueryConnection.Connection.Execute(
                procedureName,
                param: extraParam,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            DynamicParameters param = new DynamicParameters();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
            {
                param.AddDynamicParams(extraParam);
            }

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ImportAsync<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            DynamicParameters param = new DynamicParameters();

            DataTable dt = list.ToDataTableAdvance();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
            {
                param.AddDynamicParams(extraParam);
            }

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            if (extraParam == null)
            {
                extraParam = new DynamicParameters();
            }

            extraParam.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            int result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param: extraParam,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (this.newConnectionOnEachProcess)
                QueryConnection.Connection.Dispose();

            return result;
        }

        public IDataImportQuery NewDataImportQuery()
        {
            return new DataImportQuery(this);
        }

        public IDataExecuteQuery NewDataExecuteQuery()
        {
            return new DataExecuteQuery(this);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            CloseConnection();
        }
        #endregion

        #region Protected Methods

        protected void CloseConnection()
        {
            if (dbConnection != null &&
                dbConnection.State == System.Data.ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }

        #endregion
    }
}
