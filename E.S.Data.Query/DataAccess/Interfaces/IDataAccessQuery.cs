using Dapper;
using E.S.Data.Query.DataQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataAccessQuery : IDisposable
    {

        int Execute(string procedureName, DynamicParameters param, int? commandTimeout = 700);
        int Execute<P>(string procedureName, P param, int? commandTimeout = 700);
        Task<int> ExecuteAsync(string procedureName, DynamicParameters param, int? commandTimeout = 700);
        Task<int> ExecuteAsync<P>(string procedureName, P param, int? commandTimeout = 700);
        int ExecuteMultiple(string procedureName, List<DynamicParameters> param);
        int ExecuteMultiple<P>(string procedureName, params P[] param);
        Task<int> ExecuteMultipleAsync(string procedureName, List<DynamicParameters> param);
        Task<int> ExecuteMultipleAsync<P>(string procedureName, params P[] param);
        T ExecuteScalar<T, P>(string procedureName, P param, int? commandTimeout = 700);
        Task<T> ExecuteScalarAsync<T, P>(string procedureName, P param, int? commandTimeout = 700);
        T First<T, P>(string procedureName, P param);
        Task<T> FirstAsync<T, P>(string procedureName, P param);
        T FirstOrDefault<T, P>(string procedureName, P param);
        Task<T> FirstOrDefaultAsync<T, P>(string procedureName, P param);
        T First<T>(string procedureName, DynamicParameters param);
        IEnumerable<T> List<T, P>(string procedureName, P param, int? commandTimeout = 700);
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param, int? commandTimeout = 700);
        Task<IEnumerable<T>> ListAsync<T, P>(string procedureName, P param, int? commandTimeout = 700);
        Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param, int? commandTimeout = 700);
        (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2, P>(string procedureName, P param);
        int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null);
        int Import<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null);
        int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null);
        Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, DynamicParameters extraParam = null);
        Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt, object extraParam = null);
        Task<int> ImportAsync<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list, object extraParam = null);
        IEnumerable<T> List<T>(string procedureName, int? commandTimeout = 700);
        Task<IEnumerable<T>> ListAsync<T>(string procedureName, int? commandTimeout = 700);
        IDataImportQuery NewDataImportQuery();
        IDataExecuteQuery NewDataExecuteQuery();
        bool NewConnectionOnEachProcess { get; }
    }
}