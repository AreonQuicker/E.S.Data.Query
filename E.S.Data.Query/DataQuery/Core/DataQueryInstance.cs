using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;

namespace E.S.Data.Query.DataQuery.Core
{
    public partial class DataQueryInstance : IDataQueryInstance
    {
        #region Private Read only Fields

        private readonly IDataProvider dataProvider;

        #endregion

        #region Constructor

        public DataQueryInstance(
            IDataProvider dataProvider
        )
        {
            this.dataProvider = dataProvider;
        }

        #endregion

        #region IDataQueryInstance Methods

        public IDataExecuteQuery NewDataExecuteQuery(bool newConnectionOnEachProcess = true,
            bool keepConnectionClosed = true)
        {
            return new DataExecuteQuery(dataProvider.NewCommand(newConnectionOnEachProcess, keepConnectionClosed),
                true);
        }


        public IDataImportQuery NewDataImportQuery(bool newConnectionOnEachProcess = true,
            bool keepConnectionClosed = true)
        {
            return new DataImportQuery(dataProvider.NewCommand(newConnectionOnEachProcess, keepConnectionClosed), true);
        }

        #endregion
    }
}