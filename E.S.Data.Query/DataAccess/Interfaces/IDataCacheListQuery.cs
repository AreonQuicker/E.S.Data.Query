using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using E.S.Data.Query.Models;

namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataCacheListQuery : IDisposable
    {
        IDataCacheListQuery UseCache(bool useCache);

        IDataCacheListQuery SetMainCacheKey(string key);

        IDataCacheListQuery IgnoreGetCache(bool ignoreGetCache);

        //In Seconds
        IDataCacheListQuery SetDataCommandTimeOut(int? commandTimeout = 700);

        //In Seconds
        IDataCacheListQuery SetCacheTime(int cacheTime);

        IDataCacheListQuery SetDataCommand(string command);

        IDataCacheListQuery SetParameters<T>(T parameters);

        IDataCacheListQuery SetDynamicParameters(DynamicParameters parameters);
        IDataCacheListQuery SetParameters(params DataCommandParameter[] dataCommandParameters);

        IDataCacheListQuery Clear();

        IEnumerable<T> List<T>() where T : class, new();

        Task<IEnumerable<T>> ListAsync<T>() where T : class, new();
    }
}