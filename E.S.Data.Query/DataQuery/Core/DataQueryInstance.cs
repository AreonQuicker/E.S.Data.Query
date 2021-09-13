using E.S.Data.Query.DataAccess.Core;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;
using E.S.Simple.MemoryCache.Interfaces;

namespace E.S.Data.Query.DataQuery.Core
{
    public partial class DataQueryInstance : IDataQueryInstance
    {
        #region Private Read only Fields

        private readonly IDataProvider dataProvider;
        private readonly IMemoryCacheManager cacheManager;

        #endregion

        #region Constructor      

        public DataQueryInstance(
            IDataProvider dataProvider,
            IMemoryCacheManager cacheManager
            )
        {

            this.dataProvider = dataProvider;
            this.cacheManager = cacheManager;
        }

        #endregion

        #region IDataQueryInstance Methods

        public IDataImportQuery NewDataImportQuery(bool newConnectionOnEachProcess = true, bool keepConnectionClosed = true)
        {
            return new DataImportQuery(dataProvider.NewCommand(newConnectionOnEachProcess, keepConnectionClosed));
        }

        public IDataListQuery NewDataListQuery(bool newConnectionOnEachProcess = true, bool keepConnectionClosed = true)
        {
            DataCacheListQuery dataListCacheQuery =
                new DataCacheListQuery(cacheManager, dataProvider.NewCommand(newConnectionOnEachProcess, keepConnectionClosed));

            return new DataListQuery(dataListCacheQuery);
        }

        #endregion
    }
}
