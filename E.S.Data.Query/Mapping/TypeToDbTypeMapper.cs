using System;
using System.Collections.Generic;
using System.Data;

namespace E.S.Data.Query.Mapping
{
    public class TypeToDbTypeMapper
    {
        private static TypeToDbTypeMapper instance;
        public readonly Lazy<Dictionary<Type, DbType>> TypeMapping;

        public TypeToDbTypeMapper()
        {
            TypeMapping = new Lazy<Dictionary<Type, DbType>>(mappings);
        }

        private Dictionary<Type, DbType> mappings()
        {
            return new Dictionary<Type, DbType>
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
            };
        }

        public static TypeToDbTypeMapper GetInstance()
        {
            if (instance is null)
            {
                instance = new TypeToDbTypeMapper();
            }

            return instance;
        }

        public static bool TryToGetType(Type type, out DbType dbType)
        {
            dbType = default;

            if (!GetInstance().TypeMapping.Value.ContainsKey(type))
            {
                return false;
            }

            dbType = GetInstance().TypeMapping.Value[type];

            return true;
        }
    }
}
