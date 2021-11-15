using Dapper;
using E.S.Data.Query.Attributes;
using E.S.Data.Query.Mapping;
using E.S.Data.Query.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace E.S.Data.Query.Extensions
{
    public static class ObjectExtensions
    {

        public static List<DataCommandParameter> ToInputDataCommandParameters(this object item)
        {
            if (item is null)
            {
                return new List<DataCommandParameter>();
            }

            List<DataCommandParameter> dataCommandParameters = new List<DataCommandParameter>();

            PropertyInfo[] properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pi in properties)
            {
                DataAccessFieldAttribute parameterAttribute
                                   = (pi.GetCustomAttributes(typeof(DataAccessFieldAttribute), false).FirstOrDefault()) as DataAccessFieldAttribute;

                if (!((pi.GetCustomAttributes(typeof(DataAccessIgnoredFieldAttribute), false).FirstOrDefault()) is DataAccessIgnoredFieldAttribute))
                {
                    string name = parameterAttribute?.GetName() ?? pi.Name;
                    object value = pi.GetValue(item);
                    System.Type propertyType = pi.PropertyType;

                    if (TypeToDbTypeMapper.TryToGetType(propertyType, out DbType dbType))
                    {
                        dataCommandParameters.Add(new DataCommandParameter(name, dbType, value));
                    }
                }
            }               

            return dataCommandParameters;
        }

        public static DynamicParameters ToInputDynamicParameters(this object item)
        {
            if (item is null)
            {
                return null;
            }

            if (item is DynamicParameters d)
            {
                return d;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();

            List<DataCommandParameter> dataCommandParms = item.ToInputDataCommandParameters();

            foreach (DataCommandParameter dataCommandParm in dataCommandParms)
            {
                dynamicParameters.Add(dataCommandParm.Name, dataCommandParm.Value, dataCommandParm.DbType, ParameterDirection.Input);
            }

            return dynamicParameters;
        }
    }
}
