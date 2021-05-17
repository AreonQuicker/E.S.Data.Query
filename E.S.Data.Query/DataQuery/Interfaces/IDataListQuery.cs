using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataListQuery : IDataQuery
    {
        IDataListQuery IgnoreGetCache(bool ignoreGetCache);
        IDataListQuery SetMainCacheKey(string key);
        IDataListQuery UseCache(bool useCache);
        IDataListQuery SetDataCommandTimeOut(int? commandTimeout = 700);
        IDataListQuery SetCacheTime(int cacheTime);
        IEnumerable<T> List<T>() where T : class, new();
        Task<IEnumerable<T>> ListAsync<T>() where T : class, new();
    }
}
