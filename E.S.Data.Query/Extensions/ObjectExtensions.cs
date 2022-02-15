using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using E.S.Data.Query.Attributes;
using E.S.Data.Query.Mapping;
using E.S.Data.Query.Models;

namespace E.S.Data.Query.Extensions
{
    internal static class ObjectExtensions
    {
        public static List<DataCommandParameter> ToInputDataCommandParameters(this object item)
        {
            if (item is null) return new List<DataCommandParameter>();

            var dataCommandParameters = new List<DataCommandParameter>();

            var properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pi in properties)
            {
                var parameterAttribute
                    = pi.GetCustomAttributes(typeof(DataQueryFieldAttribute), false).FirstOrDefault() as
                        DataQueryFieldAttribute;

                if (!(pi.GetCustomAttributes(typeof(DataQueryIgnoredFieldAttribute), false).FirstOrDefault() is
                    DataQueryIgnoredFieldAttribute))
                {
                    var name = parameterAttribute?.GetName() ?? pi.Name;
                    var value = pi.GetValue(item);
                    var propertyType = pi.PropertyType;

                    if (TypeToDbTypeMapper.TryToGetType(propertyType, out var dbType))
                        dataCommandParameters.Add(new DataCommandParameter(name, dbType, value));
                }
            }

            return dataCommandParameters;
        }

        public static DynamicParameters ToInputDynamicParameters(this object item)
        {
            if (item is null) return null;

            if (item is DynamicParameters d) return d;

            if (item.GetType().GetCustomAttribute<DataQueryAttribute>() is null)
                return new DynamicParameters(item);

            var dynamicParameters = new DynamicParameters();

            var dataCommandParms = item.ToInputDataCommandParameters();

            foreach (var dataCommandParm in dataCommandParms)
                dynamicParameters.Add(dataCommandParm.Name, dataCommandParm.Value, dataCommandParm.DbType,
                    ParameterDirection.Input);

            return dynamicParameters;
        }
    }
}