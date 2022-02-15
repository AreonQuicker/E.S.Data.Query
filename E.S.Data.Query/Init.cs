using E.S.Data.Query.DataAccess.Core;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Core;
using E.S.Data.Query.DataQuery.Interfaces;
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
            services.AddScoped<ICreateDbConnection>(s => new CreateSQLDbConnection(connectionString));

            services.AddScoped<IDataConnection, DataConnection>();

            services.AddScoped<IDataProvider, DataProvider>();

            if (addDataAccessQueryAsTransient)
                services.AddTransient<IDataAccessQuery>(s =>
                    new DataAccessQuery(
                        s.GetService<ICreateDbConnection>(),
                        newConnectionOnEachProcess,
                        keepConnectionClosed));
            else
                services.AddScoped<IDataAccessQuery>(s =>
                    new DataAccessQuery(
                        s.GetService<ICreateDbConnection>(),
                        newConnectionOnEachProcess,
                        keepConnectionClosed));

            services.AddTransient<IDataQueryInstance, DataQueryInstance>();
        }
    }
}