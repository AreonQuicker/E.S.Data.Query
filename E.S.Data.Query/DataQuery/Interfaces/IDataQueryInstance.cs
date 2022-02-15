namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataQueryInstance
    {
        IDataImportQuery NewDataImportQuery(bool newConnectionOnEachProcess = true, bool keepConnectionClosed = true);
        IDataExecuteQuery NewDataExecuteQuery(bool newConnectionOnEachProcess = true, bool keepConnectionClosed = true);
    }
}