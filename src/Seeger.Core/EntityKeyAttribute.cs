using System;

namespace Seeger
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EntityKeyAttribute : Attribute
    {
    }
}
