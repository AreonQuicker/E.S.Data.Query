using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataQuery.Core
{
    public partial class DataQueryInstance
    {
        public class DataListQuery : DataQueryBase, IDataListQuery
        {
            #region Private Read Only Fields
            private readonly IDataCacheListQuery dataCacheListQuery;
            #endregion

            #region Private Fields         
            private string mainCacheKey = nameof(IDataListQuery);
            private bool useCache = false;
            private int cacheTime;
            private bool ignoreGetCache = false;
            private int? commandTimeout = null;
            #endregion

            #region Constructor      

            public DataListQuery(
                IDataCacheListQuery dataCacheListQuery
                )
                : base(null)
            {
                this.dataCacheListQuery = dataCacheListQuery;
            }

            #endregion     

            #region IQuery Methods      
            public IDataListQuery SetDataCommandTimeOut(int? commandTimeout = 700)
            {

                this.commandTimeout = commandTimeout;

                return this;
            }

            public IDataListQuery SetMainCacheKey(string key)
            {
                mainCacheKey = key;

                return this;
            }

            public IDataListQuery UseCache(bool useCache)
            {
                this.useCache = useCache;

                return this;
            }

            public IDataListQuery SetCacheTime(int cacheTime)
            {
                this.cacheTime = cacheTime;

                return this;
            }

            public IDataListQuery IgnoreGetCache(bool ignoreGetCache)
            {
                this.ignoreGetCache = ignoreGetCache;

                return this;
            }

            public IEnumerable<T> List<T>() where T : class, new()
            {

                var result = dataCacheListQuery
                    .Clear()
                    .SetDataCommandTimeOut(commandTimeout)
                    .UseCache(useCache)
                    .SetCacheTime(cacheTime)
                    .SetMainCacheKey(mainCacheKey)
                    .IgnoreGetCache(ignoreGetCache)
                    .SetDataCommand(actionName)
                    .SetDynamicParameters(dynamicParameters)
                    .List<T>();

                return result;
            }

            public async Task<IEnumerable<T>> ListAsync<T>() where T : class, new()
            {
                var result = await dataCacheListQuery
                   .Clear()
                   .SetDataCommandTimeOut(commandTimeout)
                   .UseCache(useCache)
                   .SetCacheTime(cacheTime)
                   .SetMainCacheKey(mainCacheKey)
                   .IgnoreGetCache(ignoreGetCache)
                   .SetDataCommand(actionName)
                   .SetDynamicParameters(dynamicParameters)
                   .ListAsync<T>();

                return result;
            }

            #endregion
        }
    }
}
