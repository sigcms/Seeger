using System;

namespace Seeger
{
    [Obsolete("Consider remove it or use Seeger.Data.Mapping.Attributes.IdAttribute instead.")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EntityKeyAttribute : Attribute
    {
    }
}
