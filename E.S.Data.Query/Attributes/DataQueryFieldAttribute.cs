using System;

namespace E.S.Data.Query.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataQueryFieldAttribute : Attribute
    {
        private readonly string name;

        public DataQueryFieldAttribute(string name = null)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}