using System.Threading.Tasks;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;

namespace E.S.Data.Query.DataQuery.Core
{
    public partial class DataQueryInstance
    {
        public class DataExecuteQuery : DataQueryBase, IDataExecuteQuery
        {
            private readonly bool disposeDataAccessQuery;

            #region Constructor

            public DataExecuteQuery(
                IDataAccessQuery dataAccessQuery,
                bool disposeDataAccessQuery = false
            )
                : base(dataAccessQuery)
            {
                this.disposeDataAccessQuery = disposeDataAccessQuery;
            }

            #endregion

            #region Private Fields

            #endregion

            #region IQuery Methods

            public void Dispose()
            {
                Clear();
                if (disposeDataAccessQuery)
                    dataAccessQuery.Dispose();
            }

            public int Execute()
            {
                return dataAccessQuery.Execute(actionName, dynamicParameters);
            }

            public Task<int> ExecuteAsnc()
            {
                return dataAccessQuery.ExecuteAsync(actionName, dynamicParameters);
            }

            #endregion
        }
    }
}