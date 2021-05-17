using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataQuery.Core
{
    public partial class DataQueryInstance
    {
        public class DataExecuteQuery : DataQueryBase, IDataExecuteQuery
        {

            #region Private Fields         
            #endregion

            #region Constructor      

            public DataExecuteQuery(
                 IDataAccessQuery dataAccessQuery
                )
                : base(dataAccessQuery)
            {

            }

            #endregion     

            #region IQuery Methods      

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
