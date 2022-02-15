using System;
using System.Collections.Generic;
using System.Data;

namespace E.S.Data.Query.Mapping
{
    internal class DbTypeToSQLDbTypeMapper
    {
        private static DbTypeToSQLDbTypeMapper instance;
        public readonly Lazy<Dictionary<DbType, SqlDbType>> TypeMapping;

        public DbTypeToSQLDbTypeMapper()
        {
            TypeMapping = new Lazy<Dictionary<DbType, SqlDbType>>(mappings);
        }

        private Dictionary<DbType, SqlDbType> mappings()
        {
            return new Dictionary<DbType, SqlDbType>
            {
                [DbType.String] = SqlDbType.VarChar,
                [DbType.Int32] = SqlDbType.Int,
                [DbType.Int16] = SqlDbType.Int,
                [DbType.Int32] = SqlDbType.Int,
                [DbType.Decimal] = SqlDbType.Decimal,
                [DbType.Double] = SqlDbType.Float,
                [DbType.DateTime] = SqlDbType.DateTime,
                [DbType.Date] = SqlDbType.Date,
                [DbType.DateTime2] = SqlDbType.DateTime2,
                [DbType.Time] = SqlDbType.Time,
                [DbType.UInt16] = SqlDbType.Int,
                [DbType.UInt32] = SqlDbType.Int,
                [DbType.UInt64] = SqlDbType.Int,
                [DbType.Boolean] = SqlDbType.Bit,
                [DbType.Guid] = SqlDbType.UniqueIdentifier
            };
        }

        public static DbTypeToSQLDbTypeMapper GetInstance()
        {
            if (instance is null) instance = new DbTypeToSQLDbTypeMapper();

            return instance;
        }

        public static bool TryToGetType(DbType dbType, out SqlDbType type)
        {
            type = default;

            if (!GetInstance().TypeMapping.Value.ContainsKey(dbType)) return false;

            type = GetInstance().TypeMapping.Value[dbType];

            return true;
        }
    }
}