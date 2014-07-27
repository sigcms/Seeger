using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class TypeExtensions
    {
        public static string AssemblyQualifiedNameWithoutVersion(this Type type)
        {
            string[] str = type.AssemblyQualifiedName.Split(',');
            return string.Format("{0},{1}", str[0], str[1]);
        }

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
