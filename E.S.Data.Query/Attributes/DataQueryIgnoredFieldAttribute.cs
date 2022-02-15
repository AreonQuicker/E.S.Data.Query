using System;

namespace E.S.Data.Query.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataQueryIgnoredFieldAttribute : Attribute
    {
    }
}