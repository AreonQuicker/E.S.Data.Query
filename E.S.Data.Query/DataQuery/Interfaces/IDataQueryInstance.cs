namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataQueryInstance
    {
        IDataImportQuery NewDataImportQuery();
        IDataListQuery NewDataListQuery();
    }
}
