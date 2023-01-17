using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using E.S.Common.Helpers.Extensions;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.Extensions;

namespace E.S.Data.Query.DataAccess.Core
{
    public class DataAccessQuery : IDataAccessQuery
    {
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
                dbConnection = createDbConnection.CreateDbConnection();
            else
                this.keepConnectionClosed = true;
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
                dbConnection.State == ConnectionState.Open)
                dbConnection.Close();
        }

        #endregion

        #region Fields

        protected IDbConnection dbConnection;
        protected IDbTransaction dbTransaction;
        protected bool newConnectionOnEachProcess;
        protected bool keepConnectionClosed;
        private readonly ICreateDbConnection createDbConnection;

        #endregion

        #region IDataAccessQuery

        public bool NewConnectionOnEachProcess => newConnectionOnEachProcess;

        public (bool IsNew, IDbConnection Connection) NewQueryConnection()
        {
            if (dbConnection != null) return (false, dbConnection);

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
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.QueryAsync<T>(
                procedureName,
                dynamicParameters,
                dbTransaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<IEnumerable<T>> ListAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            return await ListAsync<T>(procedureName, param, commandTimeout);
        }

        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, int? commandTimeout = 700)
        {
            return await ListAsync<T>(procedureName, null, commandTimeout);
        }

        #endregion

        #region ListMultiple

        public (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2>(string procedureName, object param)
        {
            return ListMultipleAsync<T1, T2>(procedureName, param).GetAwaiter().GetResult();
        }

        public (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2, P>(string procedureName, P param)
        {
            return ListMultiple<T1, T2>(procedureName, param);
        }

        public (IEnumerable<T1> First, IEnumerable<T2> Second, IEnumerable<T3> Third) ListMultiple<T1, T2, T3>(
            string procedureName, object param)
        {
            return ListMultipleAsync<T1, T2, T3>(procedureName, param).GetAwaiter().GetResult();
        }

        public (IEnumerable<T1> First, IEnumerable<T2> Second, IEnumerable<T3> Third) ListMultiple<T1, T2, T3, P>(
            string procedureName, P param)
        {
            return ListMultiple<T1, T2, T3>(procedureName, param);
        }

        #endregion

        #region ListMultipleAsync

        public async Task<(IEnumerable<T1> First, IEnumerable<T2> Second)> ListMultipleAsync<T1, T2>(
            string procedureName, object param)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            IEnumerable<T1> First;
            IEnumerable<T2> Second;

            using (var multi = await QueryConnection.Connection.QueryMultipleAsync(procedureName,
                       dynamicParameters,
                       dbTransaction,
                       commandType: CommandType.StoredProcedure))
            {
                First = multi.Read<T1>().ToList();
                Second = multi.Read<T2>().ToList();
            }

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return (First, Second);
        }

        public async Task<(IEnumerable<T1> First, IEnumerable<T2> Second)> ListMultipleAsync<T1, T2, P>(
            string procedureName, P param)
        {
            return await ListMultipleAsync<T1, T2>(procedureName, param);
        }

        public async Task<(IEnumerable<T1> First, IEnumerable<T2> Second, IEnumerable<T3> Third)>
            ListMultipleAsync<T1, T2, T3>(string procedureName, object param)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            IEnumerable<T1> First;
            IEnumerable<T2> Second;
            IEnumerable<T3> Third;

            using (var multi = await QueryConnection.Connection.QueryMultipleAsync(procedureName,
                       dynamicParameters,
                       dbTransaction,
                       commandType: CommandType.StoredProcedure))
            {
                First = multi.Read<T1>().ToList();
                Second = multi.Read<T2>().ToList();
                Third = multi.Read<T3>().ToList();
            }

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return (First, Second, Third);
        }

        public async Task<(IEnumerable<T1> First, IEnumerable<T2> Second, IEnumerable<T3> Third)> ListMultipleAsync<T1,
            T2, T3, P>(string procedureName, P param)
        {
            return await ListMultipleAsync<T1, T2, T3>(procedureName, param);
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

        public T First<T>(string procedureName)
        {
            return First<T>(procedureName, null);
        }

        #endregion

        #region FirstAsync

        public async Task<T> FirstAsync<T>(string procedureName, object param)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.QueryFirstAsync<T>(
                procedureName,
                dynamicParameters,
                dbTransaction,
                commandType: CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<T> FirstAsync<T, P>(string procedureName, P param)
        {
            return await FirstAsync<T>(procedureName, param);
        }

        public async Task<T> FirstAsync<T>(string procedureName)
        {
            return await FirstAsync<T>(procedureName, null);
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

        public T FirstOrDefault<T>(string procedureName)
        {
            return FirstOrDefault<T>(procedureName, null);
        }

        #endregion

        #region FirstOrDefaultAsync

        public async Task<T> FirstOrDefaultAsync<T>(string procedureName, object param)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.QueryFirstOrDefaultAsync<T>(
                procedureName,
                dynamicParameters,
                dbTransaction,
                commandType: CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<T> FirstOrDefaultAsync<T, P>(string procedureName, P param)
        {
            return await FirstOrDefaultAsync<T>(procedureName, param);
        }

        public async Task<T> FirstOrDefaultAsync<T>(string procedureName)
        {
            return await FirstOrDefaultAsync<T>(procedureName, null);
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

        public T ExecuteScalar<T>(string procedureName, int? commandTimeout = 700)
        {
            return ExecuteScalar<T>(procedureName, null, commandTimeout);
        }

        #endregion

        #region ExecuteScalarAsync

        public async Task<T> ExecuteScalarAsync<T>(string procedureName, object param, int? commandTimeout = 700)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.ExecuteScalarAsync<T>(
                procedureName,
                dynamicParameters,
                dbTransaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<T> ExecuteScalarAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            return await ExecuteScalarAsync<T>(procedureName, param, commandTimeout);
        }

        public async Task<T> ExecuteScalarAsync<T>(string procedureName, int? commandTimeout = 700)
        {
            return await ExecuteScalarAsync<T>(procedureName, null, commandTimeout);
        }

        #endregion

        #region Execute

        public int Execute(string procedureName, object param, int? commandTimeout = 700)
        {
            return ExecuteAsync(procedureName, param, commandTimeout)
                .GetAwaiter()
                .GetResult();
        }

        public int Execute(string procedureName, int? commandTimeout = 700)
        {
            return Execute(procedureName, null, commandTimeout);
        }

        #endregion

        #region ExecuteAsync

        public async Task<int> ExecuteAsync(string procedureName, object param, int? commandTimeout = 700)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var (IsNew, Connection) = NewQueryConnection();

            if (Connection.State == ConnectionState.Closed) Connection.Open();

            var result = await Connection.ExecuteAsync(
                procedureName,
                dynamicParameters,
                dbTransaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                Connection.Close();

            if (newConnectionOnEachProcess) Connection.Dispose();

            return result;
        }

        public async Task<int> ExecuteAsync(string procedureName, int? commandTimeout = 700)
        {
            return await ExecuteAsync(procedureName, null, commandTimeout);
        }

        #endregion

        #region ExecuteMultiple

        public int ExecuteMultiple<P>(string procedureName, params P[] param)
        {
            return ExecuteMultipleAsync(procedureName, param)
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
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param,
                dbTransaction,
                commandType: CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ExecuteMultipleAsync(string procedureName, List<DynamicParameters> param)
        {
            var (IsNew, Connection) = NewQueryConnection();

            if (Connection.State == ConnectionState.Closed) Connection.Open();

            var result = await Connection.ExecuteAsync(
                procedureName,
                param,
                dbTransaction,
                commandType: CommandType.StoredProcedure);

            if (Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                Connection.Close();

            if (newConnectionOnEachProcess) Connection.Dispose();

            return result;
        }

        #endregion

        #region Import

        public int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt,
            object extraParam = null)
        {
            return ImportAsync(procedureName, paramName, paramTableTypeName, dt, extraParam)
                .GetAwaiter()
                .GetResult();
        }

        public int Import<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list,
            object extraParam = null)
        {
            return ImportAsync(procedureName, paramName, paramTableTypeName, list, extraParam)
                .GetAwaiter()
                .GetResult();
        }

        #endregion

        #region ImportAsync

        public async Task<IEnumerable<T>> ListAsync<T, P>(string procedureName, string paramName,
            string paramTableTypeName,
            IList<P> list, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            var param = new DynamicParameters();

            param.Add($"@{paramName}", list.ToDataTableAdvance().AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null) param.AddDynamicParams(extraParam);

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.QueryAsync<T>(
                procedureName,
                param,
                dbTransaction,
                commandType: CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName,
            DataTable dt, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            var param = new DynamicParameters();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null) param.AddDynamicParams(extraParam);

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.ExecuteAsync(
                procedureName,
                param,
                dbTransaction,
                commandType: CommandType.StoredProcedure);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<int> ImportAsync<P>(string procedureName, string paramName, string paramTableTypeName,
            IList<P> list, object extraParam = null)
        {
            return await ImportAsync(procedureName, paramName, paramTableTypeName, list.ToDataTableAdvance(),
                extraParam);
        }

        #endregion

        #region ListQueryAsync

        public async Task<IEnumerable<T>> ListQueryAsync<T>(string query, object param, int? commandTimeout = 700)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.QueryAsync<T>(
                query,
                dynamicParameters,
                dbTransaction,
                commandTimeout);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<IEnumerable<T>> ListQueryAsync<T, P>(string query, P param, int? commandTimeout = 700)
        {
            return await ListQueryAsync<T>(query, param, commandTimeout);
        }

        public async Task<IEnumerable<T>> ListQueryAsync<T>(string query, int? commandTimeout = 700)
        {
            return await ListQueryAsync<T>(query, null, commandTimeout);
        }

        #endregion

        #region FirstOrDefaultQueryAsync

        public async Task<T> FirstOrDefaultQueryAsync<T>(string query, object param)
        {
            var dynamicParameters = param.ToInputDynamicParameters();

            var QueryConnection = NewQueryConnection();

            if (QueryConnection.Connection.State == ConnectionState.Closed) QueryConnection.Connection.Open();

            var result = await QueryConnection.Connection.QueryFirstOrDefaultAsync<T>(
                query,
                dynamicParameters,
                dbTransaction);

            if (QueryConnection.Connection.State == ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Connection.Close();

            if (newConnectionOnEachProcess) QueryConnection.Connection.Dispose();

            return result;
        }

        public async Task<T> FirstOrDefaultQueryAsync<T, P>(string query, P param)
        {
            return await FirstOrDefaultQueryAsync<T>(query, param);
        }

        public async Task<T> FirstOrDefaultQueryAsync<T>(string query)
        {
            return await FirstOrDefaultQueryAsync<T>(query, null);
        }

        #endregion

        #region NewQuery

        #endregion

        #endregion
    }
}