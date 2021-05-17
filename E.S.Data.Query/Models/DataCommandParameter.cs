using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace E.S.Data.Query.Models
{
    public class DataCommandParameter
    {
        public DataCommandParameter(string name, DbType dbType, object value)
        {
            Name = name;
            DbType = dbType;
            Value = value;
        }

        public string Name { get; set; }
        public System.Data.DbType DbType { get; set; }
        public object Value { get; set; }
    }
}

