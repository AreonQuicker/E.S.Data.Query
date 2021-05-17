namespace E.S.Data.Query.DataAccess.Interfaces
{
    public interface IDataConnection
    {
        string ConnectionString { get; }
        IDataProvider NewDataProvider { get; }
    }
}