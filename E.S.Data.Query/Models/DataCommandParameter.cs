using System.Data;

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
        public DbType DbType { get; set; }
        public object Value { get; set; }
    }
}