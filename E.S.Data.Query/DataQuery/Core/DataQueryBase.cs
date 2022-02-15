using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using E.S.Common.Helpers.Enums;
using E.S.Common.Helpers.Extensions;
using E.S.Data.Query.DataAccess.Interfaces;
using E.S.Data.Query.DataQuery.Interfaces;
using E.S.Data.Query.Extensions;
using E.S.Data.Query.Mapping;
using E.S.Data.Query.Models;
using Newtonsoft.Json.Linq;

namespace E.S.Data.Query.DataQuery.Core
{
    public partial class DataQueryInstance
    {
        public abstract class DataQueryBase : IDataQuery
        {
            #region Constructor

            public DataQueryBase(
                IDataAccessQuery dataAccessQuery
            )
            {
                dynamicParameters = new DynamicParameters();
                parameters = new Dictionary<string, (DbType DbType, object Value)>();
                outParameters = new Dictionary<string, DbType>();
                outParameterValues = new Dictionary<string, object>();
                this.dataAccessQuery = dataAccessQuery;
            }

            #endregion

            #region Protected Read only Fields

            #endregion

            #region Protected Fields

            protected readonly IDataAccessQuery dataAccessQuery;
            protected Dictionary<string, object> outParameterValues;
            protected Dictionary<string, DbType> outParameters;
            protected Dictionary<string, (DbType DbType, object Value)> parameters;
            protected string actionName;
            protected CaseStyleType caseStyleType = CaseStyleType.None;
            protected DynamicParameters dynamicParameters;

            #endregion

            #region Protected Methods

            protected void AddDynamicParameter(DbType dbType, string name, object value,
                ParameterDirection direction = ParameterDirection.Output, int? size = null)
            {
                if (direction == ParameterDirection.Output)
                    dynamicParameters.Add($"@{name.ToCaseStyle(caseStyleType)}", dbType: dbType, direction: direction,
                        size: size);
                else
                    dynamicParameters.Add($"@{name.ToCaseStyle(caseStyleType)}", value, dbType, direction);
            }

            protected void SetOutParameterValues()
            {
                outParameterValues = new Dictionary<string, object>();

                foreach (var outParameter in outParameters)
                {
                    var value = dynamicParameters.Get<object>(outParameter.Key.ToCaseStyle(caseStyleType));

                    if (value != null) outParameterValues.Add(outParameter.Key, value);
                }
            }

            #endregion

            #region IQuery Methods

            public IDataQuery Clear()
            {
                outParameterValues.Clear();
                dynamicParameters = new DynamicParameters();
                parameters.Clear();
                outParameters.Clear();
                actionName = null;
                caseStyleType = CaseStyleType.None;

                return this;
            }

            public IDataQuery SetParametersNameCaseStyle(CaseStyleType caseStyleType)
            {
                this.caseStyleType = caseStyleType;

                return this;
            }

            public IDataQuery SetAction(string actionName)
            {
                this.actionName = actionName;

                return this;
            }

            public IDataQuery AddOutParameter(Type type, string name, int? size = null)
            {
                if (string.IsNullOrEmpty(name)) return this;

                if (!TypeToDbTypeMapper.TryToGetType(type, out var dbType)) return this;

                AddOutParameter(dbType, name, size);

                return this;
            }

            public IDataQuery AddOutParameter(DbType dbType, string name, int? size = null)
            {
                if (string.IsNullOrEmpty(name)) return this;

                outParameters.Add(name, dbType);
                AddDynamicParameter(dbType, name, null, ParameterDirection.Output, size);

                return this;
            }

            public IDataQuery AddParameter(DbType dbType, string name, object value)
            {
                if (value is null || string.IsNullOrEmpty(name)) return this;

                parameters.Add(name, (dbType, value));
                AddDynamicParameter(dbType, name, value, ParameterDirection.Input);

                return this;
            }

            public IDataQuery AddParameter(DataCommandParameter dataCommandParameter)
            {
                if (dataCommandParameter is null) return this;

                AddParameter(dataCommandParameter.DbType, dataCommandParameter.Name, dataCommandParameter.Value);

                return this;
            }

            public IDataQuery AddParameter(Type type, string name, object value)
            {
                if (value is null || string.IsNullOrEmpty(name)) return this;

                if (!TypeToDbTypeMapper.TryToGetType(type, out var dbType)) return this;

                AddParameter(dbType, name, value);

                return this;
            }

            public IDataQuery AddParameters(params (Type Type, string Name, object Value)[] parameters)
            {
                foreach (var parameter in parameters) AddParameter(parameter.Type, parameter.Name, parameter.Value);

                return this;
            }

            public IDataQuery AddParameters(params (DbType Type, string Name, object Value)[] parameters)
            {
                foreach (var parameter in parameters) AddParameter(parameter.Type, parameter.Name, parameter.Value);

                return this;
            }

            public IDataQuery AddParameters<T>(T item)
                where T : class, new()
            {
                if (item is null) return this;

                var dataCommandParameters = item.ToInputDataCommandParameters();

                AddParameters(dataCommandParameters.ToList());

                return this;
            }

            public IDataQuery AddParametersFromObject(object item)
            {
                AddParameters(item);

                return this;
            }

            public IDataQuery AddParameter(string name, JToken item)
            {
                if (item is null) return this;

                if (!JTokenToTypeMapper.TryToGetType(item.Type, out var type)) return this;

                AddParameter(type, name, item.ToObject(type));

                return this;
            }

            public IDataQuery AddParameters(JObject item)
            {
                if (item is null) return this;

                foreach (var x in item) AddParameter(x.Key, x.Value);

                return this;
            }

            public IDataQuery AddParameters(params DataCommandParameter[] dataCommandParameters)
            {
                if (dataCommandParameters is null) return this;

                foreach (var dataCommandParameter in dataCommandParameters)
                    AddParameter(dataCommandParameter.DbType, dataCommandParameter.Name, dataCommandParameter.Value);

                return this;
            }

            public Dictionary<string, object> GetOutParameterValues()
            {
                SetOutParameterValues();

                return outParameterValues;
            }

            #endregion
        }
    }
}