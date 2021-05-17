namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataProvider
    {
        IDataCommand NewCommand(bool keepConnectionClosed = true);
        IDataCommand NewTransactionCommand();
    }
}