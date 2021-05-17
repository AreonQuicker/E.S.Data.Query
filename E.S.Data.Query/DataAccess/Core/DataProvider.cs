using E.S.Data.Query.DataAccess.Interfaces;

namespace E.S.Data.Query.DataAccess.Core
{

    public class DataProvider : IDataProvider
    {
        #region Private Read Only
        private readonly ICreateDbConnection createDbConnection;
        #endregion

        #region Constructor
        public DataProvider(ICreateDbConnection createDbConnection)
        {
            this.createDbConnection = createDbConnection;
        }

        #endregion

        #region IDataProvider

        public IDataCommand NewCommand(bool keepConnectionClosed = true)
        {
            return new DataCommand(createDbConnection, keepConnectionClosed);
        }

        public IDataCommand NewTransactionCommand()
        {
            var command = new DataCommand(createDbConnection);

            command.BeginTransaction();

            return command;
        }

        #endregion
    }
}
