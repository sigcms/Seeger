using NHibernate.Mapping.ByCode;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    [MapperFor(typeof(AssignedIdAttribute))]
    public class AssignedIdAttributeMapper : IIdAttributeMapper
    {
        public void ApplyMapping(Attribute attribute, System.Reflection.MemberInfo idProperty, Type entityType, NHibernate.Mapping.ByCode.IClassAttributesMapper mapper, MappingContext context)
        {
            mapper.Id(null, m => m.Generator(Generators.Assigned));
        }
    }
}
