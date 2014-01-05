using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityAttribute : ClassLevelAttribute
    {
        public override void ApplyMapping(NHibernate.Mapping.ByCode.IModelInspector modelInspector, Type type, NHibernate.Mapping.ByCode.IClassAttributesMapper mapper)
        {
        }
    }
}
