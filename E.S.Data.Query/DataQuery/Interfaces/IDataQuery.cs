using System;
using System.Collections.Generic;
using System.Data;
using E.S.Common.Helpers.Enums;
using E.S.Data.Query.Models;
using Newtonsoft.Json.Linq;

namespace E.S.Data.Query.DataQuery.Interfaces
{
    public interface IDataQuery
    {
        IDataQuery Clear();
        IDataQuery SetAction(string actionName);
        IDataQuery SetParametersNameCaseStyle(CaseStyleType caseStyleType);
        IDataQuery AddOutParameter(Type type, string name, int? size = null);
        IDataQuery AddOutParameter(DbType dbType, string name, int? size = null);
        IDataQuery AddParameter(Type type, string name, object value);
        IDataQuery AddParameters(params (Type Type, string Name, object Value)[] parameters);
        IDataQuery AddParameters(params (DbType Type, string Name, object Value)[] parameters);
        IDataQuery AddParameter(DbType dbType, string name, object value);
        IDataQuery AddParameters<T>(T item) where T : class, new();
        IDataQuery AddParameter(string name, JToken item);
        IDataQuery AddParameters(JObject item);
        IDataQuery AddParametersFromObject(object item);
        IDataQuery AddParameters(params DataCommandParameter[] dataCommandParameters);
        Dictionary<string, object> GetOutParameterValues();
    }
}