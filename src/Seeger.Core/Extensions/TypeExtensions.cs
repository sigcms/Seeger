using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type type)
        {
            Require.NotNull(type, "type");

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static bool HasCustomAttribute<TAttribute>(this Type type)
            where TAttribute : class
        {
            return HasCustomAttribute(type, typeof(TAttribute));
        }

        public static bool HasCustomAttribute(this Type type, Type attributeType)
        {
            return type.GetCustomAttributes(attributeType, false).Any();
        }
    }
}
