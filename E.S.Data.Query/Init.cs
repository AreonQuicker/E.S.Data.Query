using E.S.Data.Query.DataAccess.Core;
using E.S.Data.Query.DataAccess.Interfaces;
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
            bool addDataAccessQueryAsTransient = true)
        {
            services.AddSimpleMemoryCache();

            services.AddTransient<ICreateDbConnection>(s => new CreateSQLDbConnection(connectionString));

            services.AddTransient<IDataConnection, DataConnection>();

            services.AddTransient<IDataProvider, DataProvider>();

            if (addDataAccessQueryAsTransient)
            {
                services.AddTransient<IDataAccessQuery>(s =>
                    new DataAccessQuery(
                        s.GetService<ICreateDbConnection>(),
                        newConnectionOnEachProcess,
                        keepConnectionClosed));
                
                services.AddTransient<IDataCacheListQuery, DataCacheListQuery>();
            }

            else
            {
                services.AddScoped<IDataAccessQuery>(s =>
                    new DataAccessQuery(
                        s.GetService<ICreateDbConnection>(),
                        newConnectionOnEachProcess,
                        keepConnectionClosed));
                
                services.AddScoped<IDataCacheListQuery, DataCacheListQuery>();
            }
        }
    }
}