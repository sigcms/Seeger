using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GuidAttribute : IdAttribute
    {
        public override void ApplyMapping(NHibernate.Mapping.ByCode.IModelInspector modelInspector, Type entityType, string tableName, System.Reflection.MemberInfo property, NHibernate.Mapping.ByCode.IClassAttributesMapper mapper)
        {
            mapper.Id(null, m => m.Generator(Generators.Guid));
        }
    }
}
