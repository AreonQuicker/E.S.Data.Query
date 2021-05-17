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

        #region Protected Fields   
        protected IDbTransaction dbTransaction;
        private readonly ICreateDbConnection createDbConnection;
        protected bool keepConnectionClosed;
        #endregion
        protected IDbConnection dbConnection;

        #region Constructor  
        public DataAccessQuery(ICreateDbConnection createDbConnection, bool keepConnectionClosed = true)
        {
            dbTransaction = null;
            this.createDbConnection = createDbConnection;
            this.keepConnectionClosed = keepConnectionClosed;
        }
        #endregion

        #region IDataAccessQuery
       
        public bool IsKeepConnectionClosed => this.keepConnectionClosed;
        
        public IDbConnection NewQueryConnection() => dbConnection ?? createDbConnection.CreateDbConnection();

        public IEnumerable<T> List<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Query<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public IEnumerable<T> List<T>(string procedureName, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Query<T>(
                procedureName,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.QueryAsync<T>(
                procedureName,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Query<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public async Task<IEnumerable<T>> ListAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.QueryAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.QueryAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2, P>(string procedureName, P param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            IEnumerable<T1> First;
            IEnumerable<T2> Second;

            using (var multi = QueryConnection.QueryMultiple(procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: CommandType.StoredProcedure))
            {
                First = multi.Read<T1>().ToList();
                Second = multi.Read<T2>().ToList();
            }

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return (First, Second);

        }

        public T First<T, P>(string procedureName, P param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.QueryFirst<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public T First<T>(string procedureName, DynamicParameters param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.QueryFirst<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public T ExecuteScalar<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.ExecuteScalar<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public async Task<T> ExecuteScalarAsync<T, P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteScalarAsync<T>(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public int Execute<P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public int Execute(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public async Task<int> ExecuteAsync<P>(string procedureName, P param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public async Task<int> ExecuteAsync(string procedureName, DynamicParameters param, int? commandTimeout = 700)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: commandTimeout);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public int ExecuteMultiple<P>(string procedureName, params P[] param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public int ExecuteMultiple(string procedureName, List<DynamicParameters> param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public async Task<int> ExecuteMultipleAsync<P>(string procedureName, params P[] param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public async Task<int> ExecuteMultipleAsync(string procedureName, List<DynamicParameters> param)
        {
            var QueryConnection = NewQueryConnection();

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;

        }

        public int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            var param = new DynamicParameters();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
                param.AddDynamicParams(extraParam);

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public int Import<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            var param = new DynamicParameters();

            var dt = list.ToDataTableAdvance();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
                param.AddDynamicParams(extraParam);

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Execute(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            if (extraParam == null)
                extraParam = new DynamicParameters();

            extraParam.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = QueryConnection.Execute(
                procedureName,
                param: extraParam,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public async Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            var param = new DynamicParameters();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
                param.AddDynamicParams(extraParam);

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public async Task<int> ImportAsync<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            var param = new DynamicParameters();

            var dt = list.ToDataTableAdvance();

            param.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (extraParam != null)
                param.AddDynamicParams(extraParam);

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteAsync(
                procedureName,
                param: param,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

            return result;
        }

        public async Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null)
        {
            var QueryConnection = NewQueryConnection();

            if (extraParam == null)
                extraParam = new DynamicParameters();

            extraParam.Add($"@{paramName}", dt.AsTableValuedParameter(paramTableTypeName));

            if (QueryConnection.State == System.Data.ConnectionState.Closed)
                QueryConnection.Open();

            var result = await QueryConnection.ExecuteAsync(
                procedureName,
                param: extraParam,
                transaction: dbTransaction,
                commandType: System.Data.CommandType.StoredProcedure);

            if (QueryConnection.State == System.Data.ConnectionState.Open
                && keepConnectionClosed)
                QueryConnection.Close();

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
    }
}
