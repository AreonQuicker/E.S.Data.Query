using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace E.S.Data.Query.Mapping
{
    public class JTokenToTypeMapper
    {
        private static JTokenToTypeMapper instance;
        public readonly Lazy<Dictionary<JTokenType, Type>> TypeMapping;

        public JTokenToTypeMapper()
        {
            TypeMapping = new Lazy<Dictionary<JTokenType, Type>>(mappings);
        }

        private Dictionary<JTokenType, Type> mappings()
        {
            return new Dictionary<JTokenType, Type>
            {
                [JTokenType.String] = typeof(string),
                [JTokenType.Integer] = typeof(int),
                [JTokenType.Float] = typeof(decimal),
                [JTokenType.Boolean] = typeof(bool),
                [JTokenType.Guid] = typeof(Guid),
                [JTokenType.Date] = typeof(DateTime)

            };
        }

        public static JTokenToTypeMapper GetInstance()
        {
            if (instance is null)
            {
                instance = new JTokenToTypeMapper();
            }

            return instance;
        }

        public static bool TryToGetType(JTokenType jTokenType, out Type type)
        {
            type = default;

            if (!GetInstance().TypeMapping.Value.ContainsKey(jTokenType))
            {
                return false;
            }

            type = GetInstance().TypeMapping.Value[jTokenType];

            return true;
        }
    }
}
