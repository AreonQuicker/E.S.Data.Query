namespace E.S.Data.Query.Attributes
{

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class DataAccessFieldAttribute : System.Attribute
    {
        private readonly string Name;

        public DataAccessFieldAttribute(string name = null)
        {
            Name = name;
        }

        public string GetName()
        {
            return Name;
        }
    }
}
