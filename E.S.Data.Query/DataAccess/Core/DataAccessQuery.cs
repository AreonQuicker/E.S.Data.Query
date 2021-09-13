using Dapper;
using E.S.Common.Helpers.Extensions;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;
using E.S.Data.Query.Extensions;
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

        public (bool IsNew, IDbConnection Connection) NewQueryConnection()
        {
            if (dbConnection != null)
            {
                return (false, dbConnection);
            }

            return (true, createDbConnection.CreateDbConnection());
        }

        #region List
        public IEnumerable<T> List<T>(string procedureName, object param, int? commandTimeout = 700)
        {
            return ListAsync<T>(procedureName, param, commandTimeout)
                .GetAwaiter()
                .GetResult();

        }

        public IEnumerable<T> List<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            return List<T>(procedureName, param, commandTimeout);
        }

        public IEnumerable<T> List<T>(string procedureName, int? commandTimeout = 700)
        {
            return List<T>(procedureName, null, commandTimeout);
        }
        #endregion

        #region ListAsync
        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, object param, int? commandTimeout = 700)
        {

            DynamicParameters dynamicParameters = param.ToInputDynamicParameters();

            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T> result = await QueryConnection.Connection.QueryAsync<T>(
                procedureName,
                param: dynamicParameters,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return result;

        }

        public Task<IEnumerable<T>> ListAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {

            return ListAsync<T>(procedureName, param, commandTimeout);

        }

        public Task<IEnumerable<T>> ListAsync<T>(string procedureName, int? commandTimeout = 700)
        {
            return ListAsync<T>(procedureName, null, commandTimeout);
        }

        #endregion

        #region ListMultiple

        public (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2>(string procedureName, object param)
        {
            DynamicParameters dynamicParameters = param.ToInputDynamicParameters();

            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            IEnumerable<T1> First;
            IEnumerable<T2> Second;

            using (SqlMapper.GridReader multi = QueryConnection.Connection.QueryMultiple(procedureName,
                param: dynamicParameters,
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

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return (First, Second);

        }

        public (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2, P>(string procedureName, P param)
        {
            return ListMultiple<T1, T2>(procedureName, param);

        }
        #endregion

        #region First
        public T First<T>(string procedureName, object param)
        {
            return FirstAsync<T>(procedureName, param)
                .GetAwaiter()
                .GetResult();
        }

        public T First<T, P>(string procedureName, P param)
        {
            return First<T>(procedureName, param);
        }

        #endregion

        #region FirstAsync
        public async Task<T> FirstAsync<T>(string procedureName, object param)
        {
            DynamicParameters dynamicParameters = param.ToInputDynamicParameters();

            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = await QueryConnection.Connection.QueryFirstAsync<T>(
                procedureName,
                param: dynamicParameters,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return result;

        }

        public Task<T> FirstAsync<T, P>(string procedureName, P param)
        {
            return FirstAsync<T>(procedureName, param);
        }
        #endregion

        #region FirstOrDefault
        public T FirstOrDefault<T>(string procedureName, object param)
        {
            return FirstOrDefaultAsync<T>(procedureName, param)
                .GetAwaiter()
                .GetResult();
        }

        public T FirstOrDefault<T, P>(string procedureName, P param)
        {
            return FirstOrDefault<T>(procedureName, param);
        }

        #endregion

        #region FirstOrDefaultAsync
        public async Task<T> FirstOrDefaultAsync<T>(string procedureName, object param)
        {
            DynamicParameters dynamicParameters = param.ToInputDynamicParameters();

            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = await QueryConnection.Connection.QueryFirstOrDefaultAsync<T>(
                procedureName,
                param: dynamicParameters,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return result;

        }
        public Task<T> FirstOrDefaultAsync<T, P>(string procedureName, P param)
        {
            return FirstOrDefaultAsync<T>(procedureName, param);
        }

        #endregion      

        #region ExecuteScalar
        public T ExecuteScalar<T>(string procedureName, object param, int? commandTimeout = 700)
        {

            return ExecuteScalarAsync<T>(procedureName, param, commandTimeout)
                .GetAwaiter()
                .GetResult();
        }

        public T ExecuteScalar<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            return ExecuteScalar<T>(procedureName, param, commandTimeout);
        }
        #endregion

        #region ExecuteScalarAsync
        public async Task<T> ExecuteScalarAsync<T>(string procedureName, object param, int? commandTimeout = 700)
        {
            DynamicParameters dynamicParameters = param.ToInputDynamicParameters();

            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Closed)
            {
                QueryConnection.Connection.Open();
            }

            T result = await QueryConnection.Connection.ExecuteScalarAsync<T>(
                procedureName,
                param: dynamicParameters,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                QueryConnection.Connection.Close();
            }

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return result;
        }

        public Task<T> ExecuteScalarAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            return ExecuteScalarAsync<T>(procedureName, param, commandTimeout);
        }
        #endregion

        #region Execute
        public int Execute(string procedureName, object param, int? commandTimeout = 700)
        {
            return ExecuteAsync(procedureName, param, commandTimeout)
                .GetAwaiter()
                .GetResult();
        }
        #endregion

        #region ExecuteAsync
        public async Task<int> ExecuteAsync(string procedureName, object param, int? commandTimeout = 700)
        {
            DynamicParameters dynamicParameters = param.ToInputDynamicParameters();

            (bool IsNew, IDbConnection Connection) = NewQueryConnection();

            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }

            int result = await Connection.ExecuteAsync(
                procedureName,
                param: dynamicParameters,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                Connection.Close();
            }

            if (newConnectionOnEachProcess)
            {
                Connection.Dispose();
            }

            return result;
        }
        #endregion

        #region ExecuteMultiple
        public int ExecuteMultiple<P>(string procedureName, params P[] param)
        {
            return ExecuteMultipleAsync<P>(procedureName, param)
               .GetAwaiter()
               .GetResult();

        }

        public int ExecuteMultiple(string procedureName, List<DynamicParameters> param)
        {
            return ExecuteMultipleAsync(procedureName, param)
                .GetAwaiter()
                .GetResult();

        }
        #endregion

        #region ExecuteMultipleAsync
        public async Task<int> ExecuteMultipleAsync<P>(string procedureName, params P[] param)
        {
            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

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

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return result;

        }

        public async Task<int> ExecuteMultipleAsync(string procedureName, List<DynamicParameters> param)
        {
            (bool IsNew, IDbConnection Connection) = NewQueryConnection();

            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }

            int result = await Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                Connection.Close();
            }

            if (newConnectionOnEachProcess)
            {
                Connection.Dispose();
            }

            return result;

        }
        #endregion

        #region Import
        public int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null)
        {
            return ImportAsync(procedureName, paramName, paramTableTypeName, dt, extraParam)
                   .GetAwaiter()
                   .GetResult();
        }

        public int Import<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null)
        {
            return ImportAsync<P>(procedureName, paramName, paramTableTypeName, list, extraParam)
                .GetAwaiter()
                .GetResult();
        }

        public int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null)
        {
            return ImportAsync(procedureName, paramName, paramTableTypeName, dt, extraParam)
                .GetAwaiter()
                .GetResult();
        }
        #endregion

        #region ImportAsync
        public async Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null)
        {
            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

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

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return result;
        }

        public async Task<int> ImportAsync<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null)
        {
            (bool IsNew, IDbConnection Connection) = NewQueryConnection();

            DynamicParameters param = new DynamicParameters();

            DataTable dt = list.ToDataTableAdvance();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
            {
                param.AddDynamicParams(extraParam);
            }

            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }

            int result = await Connection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (Connection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
            {
                Connection.Close();
            }

            if (newConnectionOnEachProcess)
            {
                Connection.Dispose();
            }

            return result;
        }

        public async Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null)
        {
            (bool IsNew, IDbConnection Connection) QueryConnection = NewQueryConnection();

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

            if (newConnectionOnEachProcess)
            {
                QueryConnection.Connection.Dispose();
            }

            return result;
        }
        #endregion

        #region NewQuery
        public IDataImportQuery NewDataImportQuery()
        {
            return new DataImportQuery(this);
        }

        public IDataExecuteQuery NewDataExecuteQuery()
        {
            return new DataExecuteQuery(this);
        }
        #endregion

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
