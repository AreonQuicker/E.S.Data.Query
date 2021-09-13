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

        public IDataCommand NewCommand(bool newConnectionOnEachProcess = true, bool keepConnectionClosed = true)
        {
            return new DataCommand(createDbConnection, newConnectionOnEachProcess, keepConnectionClosed);
        }

        public IDataCommand NewTransactionCommand()
        {
            DataCommand command = new DataCommand(createDbConnection, false, false);

            command.BeginTransaction();

            return command;
        }

        #endregion
    }
}
