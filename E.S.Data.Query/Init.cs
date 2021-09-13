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
        public static void AddDataQuery(
            this IServiceCollection services,
            string connectionString,
            bool newConnectionOnEachProcess = true,
            bool keepConnectionClosed = true,
            bool addAsTransient = true)
        {
            services.AddSimpleMemoryCache();

            services.AddScoped<ICreateDbConnection>(s => new CreateSQLDbConnection(connectionString));

            services.AddScoped<IDataConnection, DataConnection>();

            services.AddScoped<IDataProvider, DataProvider>();

            if (addAsTransient)
            {
                services.AddTransient<IDataAccessQuery>(s =>
                new DataAccessQuery(
                    s.GetService<ICreateDbConnection>(),
                    newConnectionOnEachProcess,
                    keepConnectionClosed));
            }
            else
            {
                services.AddScoped<IDataAccessQuery>(s =>
                new DataAccessQuery(
                   s.GetService<ICreateDbConnection>(),
                   newConnectionOnEachProcess,
                   keepConnectionClosed));
            }

            services.AddTransient<IDataCacheListQuery, DataCacheListQuery>();
            services.AddTransient<IDataQueryInstance, DataQueryInstance>();
        }
    }
}
