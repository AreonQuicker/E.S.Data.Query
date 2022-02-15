using System;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataExecuteQuery : IDataQuery, IDisposable
    {
        int Execute();

        Task<int> ExecuteAsnc();
    }
}