using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Data.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AssignedIdAttribute : IdAttribute
    {
        public override void ApplyMapping(IModelInspector modelInspector, Type entityType, string tableName, MemberInfo property, IClassAttributesMapper mapper)
        {
            mapper.Id(null, m => m.Generator(Generators.Assigned));
        }
    }
}
