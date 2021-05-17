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
        private readonly ICacheManager cacheManager;

        #endregion

        #region Constructor      

        public DataQueryInstance(
            IDataProvider dataProvider,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor,
            ICacheManager cacheManager
            )
        {

            this.dataProvider = dataProvider;
            this.cacheManager = cacheManager;
        }

        #endregion

        #region IDataQueryInstance Methods

        public IDataImportQuery NewDataImportQuery()
        {
            return new DataImportQuery(dataProvider.NewCommand());
        }

        public IDataListQuery NewDataListQuery()
        {
            var dataListCacheQuery = new DataCacheListQuery(cacheManager, dataProvider.NewCommand());

            return new DataListQuery(dataListCacheQuery);
        }

        #endregion
    }
}
