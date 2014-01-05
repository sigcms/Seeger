using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Attributes
{
    public abstract class ClassLevelAttribute : Attribute
    {
        public abstract void ApplyMapping(IModelInspector modelInspector, Type type, IClassAttributesMapper mapper);
    }
}
