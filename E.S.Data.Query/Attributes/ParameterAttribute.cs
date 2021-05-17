namespace E.S.Data.Query.Attributes
{

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class ParameterAttribute : System.Attribute
    {
        private readonly string Name;

        public ParameterAttribute(string name = null)
        {
            Name = name;
        }

        public string GetName()
        {
            return Name;
        }
    }
}
