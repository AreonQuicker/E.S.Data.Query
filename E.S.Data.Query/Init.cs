using E.S.Data.Query.DataAccess.Core;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Core;
using E.S.Data.Query.DataQuery.Interfaces;
using E.S.Simple.MemoryCache;
using Microsoft.Extensions.DependencyInjection;

namespace E.S.Data.Query
{
    public static class Init
    {
        public static void AddDataQuery(this IServiceCollection services, string connectionString)
        {
            services.AddSimpleMemoryCache();

            services.AddSingleton<ICreateDbConnection>(s => new CreateSQLDbConnection(connectionString));

            services.AddTransient<IDataConnection, DataConnection>();

            services.AddTransient<IDataProvider, DataProvider>();

            services.AddTransient<IDataAccessQuery, DataAccessQuery>();

            services.AddTransient<IDataCacheListQuery, DataCacheListQuery>();

            services.AddTransient<IDataQueryInstance, DataQueryInstance>();
        }
    }
}
