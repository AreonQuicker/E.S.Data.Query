using Dapper;
using E.S.Data.Query.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataCacheListQuery
    {
        IDataCacheListQuery UseCache(bool useCache);

        IDataCacheListQuery SetMainCacheKey(string key);

        IDataCacheListQuery IgnoreGetCache(bool ignoreGetCache);
        //In Seconds
        IDataCacheListQuery SetDataCommandTimeOut(int? commandTimeout = 700);
        //In minutes
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
