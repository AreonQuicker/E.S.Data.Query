using E.S.Common.Helpers.Extensions;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace E.S.Data.Query.DataQuery.Core
{

    public partial class DataQueryInstance
    {
        public class DataImportQuery : DataQueryBase, IDataImportQuery
        {

            #region Private Fields         
            private string parameterName;
            private string parameterTableTypeName;
            private DataTable dataTable;
            #endregion

            #region Constructor      

            public DataImportQuery(
                IDataAccessQuery dataAccessQuery
                )
                : base(dataAccessQuery)
            {

            }

            #endregion  

            #region IQuery Methods        

            public IDataImportQuery SetImportValue(DataTable dataTable, string parameterName, string parameterTableTypeName)
            {
                this.parameterName = parameterName;
                this.parameterTableTypeName = parameterTableTypeName;
                this.dataTable = dataTable;

                return this;
            }

            public IDataImportQuery SetImportValue<T>(IList<T> list, string parameterName, string parameterTableTypeName)
            {
                this.parameterName = parameterName;
                this.parameterTableTypeName = parameterTableTypeName;
                dataTable = list.ToDataTableAdvance();

                return this;
            }

            public int Import()
            {
                return dataAccessQuery.Import(actionName, parameterName, parameterTableTypeName, dataTable, dynamicParameters);
            }

            public async Task<int> ImportAsync()
            {
                var result = await dataAccessQuery.ImportAsync(actionName, parameterName, parameterTableTypeName, dataTable, dynamicParameters);

                return result;
            }

            #endregion
        }
    }
}
