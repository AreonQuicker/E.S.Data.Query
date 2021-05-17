using System.Threading.Tasks;

namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataExecuteQuery : IDataQuery
    {
        int Execute();

        Task<int> ExecuteAsnc();
    }
}
