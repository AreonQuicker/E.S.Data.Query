using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.Extensions;
using E.S.Data.Query.Models;
using E.S.Simple.MemoryCache.Interfaces;

namespace E.S.Data.Query.DataAccess.Core
{
    public class DataCacheListQuery : IDataCacheListQuery
    {
        private readonly IDataAccessQuery _dataAccessQuery;
        private readonly IMemoryCacheManager _memoryCacheManager;

        public DataCacheListQuery(IDataAccessQuery dataAccessQuery, IMemoryCacheManager memoryCacheManager)
        {
            _dataAccessQuery = dataAccessQuery;
            _memoryCacheManager = memoryCacheManager;
            _dynamicParameters = new DynamicParameters();
        }

        private bool _useCache = false;
        private string _command;
        private int? _commandTimeout = 700;
        private int _cacheTime;
        private DynamicParameters _dynamicParameters;
        private bool _ignoreGetCache;
        private string _mainCacheKey;

        public void Dispose()
        {
            _dataAccessQuery.Dispose();
        }

        public IDataCacheListQuery UseCache(bool useCache)
        {
            _useCache = useCache;
            return this;
        }

        public IDataCacheListQuery SetMainCacheKey(string key)
        {
            _mainCacheKey = key;

            return this;
        }

        public IDataCacheListQuery IgnoreGetCache(bool ignoreGetCache)
        {
            _ignoreGetCache = ignoreGetCache;

            return this;
        }

        public IDataCacheListQuery SetDataCommandTimeOut(int? commandTimeout)
        {
            _commandTimeout = commandTimeout;

            return this;
        }

        public IDataCacheListQuery SetCacheTime(int cacheTime)
        {
            _cacheTime = cacheTime;
            return this;
        }

        public IDataCacheListQuery SetDataCommand(string command)
        {
            _command = command;

            return this;
        }

        public IDataCacheListQuery SetParameters<T>(T parameters)
        {
            var dataCommandParameters = parameters.ToInputDataCommandParameters();

            foreach (var dataCommandParameter in dataCommandParameters)
            {
                _dynamicParameters.Add($"@{dataCommandParameter}", dataCommandParameter.Value,
                    dataCommandParameter.DbType, ParameterDirection.Input);
            }

            return this;
        }

        public IDataCacheListQuery SetDynamicParameters(DynamicParameters parameters)
        {
            _dynamicParameters = parameters;

            return this;
        }

        public IDataCacheListQuery SetParameters(params DataCommandParameter[] dataCommandParameters)
        {
            foreach (var dataCommandParameter in dataCommandParameters)
            {
                _dynamicParameters.Add($"@{dataCommandParameter.Name}", dataCommandParameter.Value,
                    dataCommandParameter.DbType, ParameterDirection.Input);
            }

            return this;
        }

        public IDataCacheListQuery Clear()
        {
            return this;
        }

        public IEnumerable<T> List<T>() where T : class, new()
        {
            return ListAsync<T>().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<T>> ListAsync<T>() where T : class, new()
        {
            var cacheKey = $"{_mainCacheKey}_{_command}";

            if (_useCache && !_ignoreGetCache)
            {
                var cacheData = _memoryCacheManager.Get<IEnumerable<T>>(cacheKey);

                if (cacheData != null && cacheData.Any())
                    return cacheData;
            }

            var data = (await _dataAccessQuery.ListAsync<T>(_command, _dynamicParameters, _commandTimeout)).ToList();

            if (_useCache && data.Any())
                _memoryCacheManager.Set(cacheKey, data, _cacheTime * 60);

            return data;
        }
    }
}