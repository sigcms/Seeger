using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MapperForAttribute : Attribute
    {
        public Type AttributeType { get; private set; }

        public MapperForAttribute(Type attributeType)
        {
            AttributeType = attributeType;
        }
    }
}
