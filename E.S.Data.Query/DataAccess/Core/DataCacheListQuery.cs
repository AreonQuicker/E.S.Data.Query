using Dapper;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.Models;
using E.S.Simple.MemoryCache.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataAccess.Core
{

    public class DataCacheListQuery : IDataCacheListQuery
    {
        #region Private Read Only Fields
        private readonly ICacheManager cacheManager;
        private readonly IDataAccessQuery dataAccessQuery;
        #endregion

        #region Private Fields
        private int? cacheTime = null;
        private string mainCacheyKey = null;
        private string command = null;
        private DynamicParameters parameters = null;
        private bool useCache = true;
        private bool ignoreGetCache = false;
        private int? commandTimeout = null;
        #endregion

        #region Constructor
        public DataCacheListQuery(
          ICacheManager cacheManager,
          IDataAccessQuery dataAccessQuery
          )
        {

            this.cacheManager = cacheManager;
            this.dataAccessQuery = dataAccessQuery;
        }
        #endregion 

        #region Private Methods

        private string GetHashCode(string[] array)
        {
            return string.Join(",", array);
        }

        #endregion

        #region IDataCacheListQuery Methods
        public IDataCacheListQuery Clear()
        {
            commandTimeout = null;
            useCache = true;
            parameters = null;
            command = null;
            cacheTime = null;
            mainCacheyKey = null;

            return this;
        }

        public IDataCacheListQuery SetDataCommandTimeOut(int? commandTimeout = 700)
        {
            this.commandTimeout = commandTimeout;

            return this;
        }

        public IDataCacheListQuery SetCacheTime(int cacheTime)
        {
            this.cacheTime = cacheTime;

            return this;
        }

        public IDataCacheListQuery UseCache(bool useCache)
        {
            this.useCache = useCache;

            return this;
        }

        public IDataCacheListQuery IgnoreGetCache(bool ignoreGetCache)
        {
            this.ignoreGetCache = ignoreGetCache;

            return this;
        }

        public IDataCacheListQuery SetDataCommand(string command)
        {
            this.command = command;

            return this;
        }

        public IDataCacheListQuery SetParameters<T>(T parameters)
        {
            if (parameters != null)
                this.parameters = new DynamicParameters(parameters);

            return this;
        }

        public IDataCacheListQuery SetDynamicParameters(DynamicParameters parameters)
        {
            if (parameters is null)
                return this;

            this.parameters = parameters;

            return this;
        }

        public IDataCacheListQuery SetParameters(params DataCommandParameter[] dataCommandParameters)
        {

            if (dataCommandParameters is null)
                return this;

            parameters = new DynamicParameters();

            foreach (var dataCommandParameter in dataCommandParameters)
                parameters.Add(dataCommandParameter.Name, dataCommandParameter.Value, dataCommandParameter.DbType);

            return this;
        }

        public IDataCacheListQuery SetMainCacheKey(string key)
        {
            mainCacheyKey = key;

            return this;
        }

        public IEnumerable<T> List<T>() where T : class, new()
        {
            var cacheKeys = GetCacheKeys();
            var hashCode = GetHashCode(cacheKeys.ToArray());

            if (useCache && !ignoreGetCache && cacheManager != null)
            {
                if (cacheManager.IsSet(hashCode))
                    return cacheManager.Get<IList<T>>(hashCode);
            }

            var values = GetValues<T>();

            if (useCache && cacheManager != null)
            {
                if (values != null)
                    cacheManager.Set(hashCode, values, GetCacheTime());
            }

            return values;
        }

        public async Task<IEnumerable<T>> ListAsync<T>() where T : class, new()
        {
            var cacheKeys = GetCacheKeys();
            var hashCode = GetHashCode(cacheKeys.ToArray());

            if (useCache && !ignoreGetCache && cacheManager != null)
            {
                if (cacheManager.IsSet(hashCode))
                    return cacheManager.Get<IList<T>>(hashCode);
            }

            var values = await GetValuesAsync<T>();

            if (useCache && cacheManager != null)
            {
                if (values != null)
                    cacheManager.Set(hashCode, values, GetCacheTime());
            }

            return values;
        }

        public void Dispose()
        {
            Clear();
        }

        #endregion

        #region Virtual Methods

        public virtual IEnumerable<T> GetValues<T>() where T : class, new()
        {
            if (parameters == null)
                parameters = new DynamicParameters();

            return dataAccessQuery.List<T>(command, parameters, commandTimeout);
        }

        public virtual Task<IEnumerable<T>> GetValuesAsync<T>() where T : class, new()
        {
            if (parameters == null)
                parameters = new DynamicParameters();

            return dataAccessQuery.ListAsync<T>(command, parameters, commandTimeout);
        }

        public virtual IList<string> GetCacheKeys()
        {
            var keys = new List<string>();

            if (!string.IsNullOrEmpty(mainCacheyKey))
                keys.Add($"mainCacheyKey={mainCacheyKey}");

            if (parameters != null
                && (parameters.ParameterNames?.Any() ?? false))
            {

                foreach (var name in parameters.ParameterNames)
                {
                    var value = parameters.Get<object>(name);

                    keys.Add($"parameterName:{name}=parameterValue:{value ?? ""}");
                }
            }

            if (!string.IsNullOrEmpty(command))
                keys.Add($"command={command}");

            return keys;
        }

        public virtual int GetCacheTime()
        {
            if (cacheTime.HasValue)
                return cacheTime.Value;

            return 0;
        }
        #endregion
    }
}
