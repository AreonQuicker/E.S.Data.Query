namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataProvider
    {
        IDataCommand NewCommand(bool newConnectionOnEachProcess = true, bool keepConnectionClosed = true);
        IDataCommand NewTransactionCommand();
    }
}