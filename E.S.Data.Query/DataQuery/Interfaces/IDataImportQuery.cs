using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataImportQuery : IDataQuery
    {
        IDataImportQuery SetImportValue(DataTable dataTable, string parameterName, string parameterTableTypeName);

        IDataImportQuery SetImportValue<T>(IList<T> list, string parameterName, string parameterTableTypeName);

        int Import();

        Task<int> ImportAsync();
    }
}
