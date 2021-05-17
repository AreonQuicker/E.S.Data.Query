using E.S.Data.Query.DataAccess.Interfaces;

namespace E.S.Data.Query.DataAccess.Core
{
    public class DataConnection : IDataConnection
    {
        #region Private Read Only Fields

        private readonly ICreateDbConnection createDbConnection;

        #endregion

        #region Constructor

        public DataConnection(ICreateDbConnection createDbConnection)
        {
            this.createDbConnection = createDbConnection;
        }

        #endregion

        #region IDataConnection

        public string ConnectionString => createDbConnection.ConnectionString;

        public IDataProvider NewDataProvider => new DataProvider(createDbConnection);

        #endregion
    }
}