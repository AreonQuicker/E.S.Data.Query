using E.S.Data.Query.Models;
using System.Collections.Generic;

namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataQuery
    {
        IDataQuery Clear();
        IDataQuery SetAction(string actionName);
        IDataQuery SetParametersNameCaseStyle(Common.Helpers.Enums.CaseStyleType caseStyleType);
        IDataQuery AddOutParameter(System.Type type, string name, int? size = null);
        IDataQuery AddOutParameter(System.Data.DbType dbType, string name, int? size = null);
        IDataQuery AddParameter(System.Type type, string name, object value);
        IDataQuery AddParameters(params (System.Type Type, string Name, object Value)[] parameters);
        IDataQuery AddParameters(params (System.Data.DbType Type, string Name, object Value)[] parameters);
        IDataQuery AddParameter(System.Data.DbType dbType, string name, object value);
        IDataQuery AddParameters<T>(T item) where T : class, new();
        IDataQuery AddParameter(string name, Newtonsoft.Json.Linq.JToken item);
        IDataQuery AddParameters(Newtonsoft.Json.Linq.JObject item);
        IDataQuery AddParametersFromObject(object item);
        IDataQuery AddParameters(params DataCommandParameter[] dataCommandParameters);
        Dictionary<string, object> GetOutParameterValues();
    }
}
