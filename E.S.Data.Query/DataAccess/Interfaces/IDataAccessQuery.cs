using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using E.S.Data.Query.DataQuery.Interfaces;

namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataAccessQuery : IDisposable
    {
        bool NewConnectionOnEachProcess { get; }

        //Execute
        int Execute(string procedureName, object param, int? commandTimeout = 700);
        int Execute(string procedureName, int? commandTimeout = 700);

        //ExecuteAsync
        Task<int> ExecuteAsync(string procedureName, object param, int? commandTimeout = 700);
        Task<int> ExecuteAsync(string procedureName, int? commandTimeout = 700);

        //ExecuteMultiple
        int ExecuteMultiple(string procedureName, List<DynamicParameters> param);

        int ExecuteMultiple<P>(string procedureName, params P[] param);

        //ExecuteMultipleAsync
        Task<int> ExecuteMultipleAsync(string procedureName, List<DynamicParameters> param);

        Task<int> ExecuteMultipleAsync<P>(string procedureName, params P[] param);

        //ExecuteScalar
        T ExecuteScalar<T, P>(string procedureName, P param, int? commandTimeout = 700);
        T ExecuteScalar<T>(string procedureName, object param, int? commandTimeout = 700);

        T ExecuteScalar<T>(string procedureName, int? commandTimeout = 700);

        //ExecuteScalarAsync
        Task<T> ExecuteScalarAsync<T, P>(string procedureName, P param, int? commandTimeout = 700);
        Task<T> ExecuteScalarAsync<T>(string procedureName, object param, int? commandTimeout = 700);

        Task<T> ExecuteScalarAsync<T>(string procedureName, int? commandTimeout = 700);

        //First
        T First<T, P>(string procedureName, P param);
        T First<T>(string procedureName, object param);
        T First<T>(string procedureName);

        //FirstAsync
        Task<T> FirstAsync<T, P>(string procedureName, P param);
        Task<T> FirstAsync<T>(string procedureName, object param);

        Task<T> FirstAsync<T>(string procedureName);

        //FirstOrDefault
        T FirstOrDefault<T, P>(string procedureName, P param);
        T FirstOrDefault<T>(string procedureName, object param);
        T FirstOrDefault<T>(string procedureName);

        //FirstOrDefaultAsync
        Task<T> FirstOrDefaultAsync<T, P>(string procedureName, P param);
        Task<T> FirstOrDefaultAsync<T>(string procedureName, object param);
        Task<T> FirstOrDefaultAsync<T>(string procedureName);

        //List
        IEnumerable<T> List<T>(string procedureName, int? commandTimeout = 700);
        IEnumerable<T> List<T, P>(string procedureName, P param, int? commandTimeout = 700);
        IEnumerable<T> List<T>(string procedureName, object param, int? commandTimeout = 700);

        //ListAsync
        Task<IEnumerable<T>> ListAsync<T>(string procedureName, int? commandTimeout = 700);
        Task<IEnumerable<T>> ListAsync<T, P>(string procedureName, P param, int? commandTimeout = 700);
        Task<IEnumerable<T>> ListAsync<T>(string procedureName, object param, int? commandTimeout = 700);

        //ListMultiple
        (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2, P>(string procedureName, P param);

        (IEnumerable<T1> First, IEnumerable<T2> Second) ListMultiple<T1, T2>(string procedureName, object param);

        //Import
        int Import(string procedureName, string paramName, string paramTableTypeName, DataTable dt,
            object extraParam = null);

        int Import<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list,
            object extraParam = null);

        //ImportAsync
        Task<int> ImportAsync(string procedureName, string paramName, string paramTableTypeName, DataTable dt,
            object extraParam = null);

        Task<int> ImportAsync<P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list,
            object extraParam = null);
        
        Task<IEnumerable<T>> ListAsync<T,P>(string procedureName, string paramName, string paramTableTypeName, IList<P> list,
            object extraParam = null);

        //ListQueryAsync
        Task<IEnumerable<T>> ListQueryAsync<T>(string query, int? commandTimeout = 700);
        Task<IEnumerable<T>> ListQueryAsync<T, P>(string query, P param, int? commandTimeout = 700);
        Task<IEnumerable<T>> ListQueryAsync<T>(string query, object param, int? commandTimeout = 700);

        //FirstOrDefaultQueryAsync
        Task<T> FirstOrDefaultQueryAsync<T, P>(string query, P param);
        Task<T> FirstOrDefaultQueryAsync<T>(string query, object param);
        Task<T> FirstOrDefaultQueryAsync<T>(string query);

        //NewIQuery
        IDataImportQuery NewDataImportQuery();
        IDataExecuteQuery NewDataExecuteQuery();
    }
}