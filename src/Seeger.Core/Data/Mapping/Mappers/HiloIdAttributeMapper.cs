using NHibernate.Mapping.ByCode;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    [MapperFor(typeof(HiloIdAttribute))]
    public class HiloIdAttributeMapper : IIdAttributeMapper
    {
        public void ApplyMapping(Attribute attribute, System.Reflection.MemberInfo idProperty, Type entityType, IClassAttributesMapper mapper, MappingContext context)
        {
            mapper.Id(null, IdMappings.HighLowId(context.Conventions.GetTableName(entityType)));
        }
    }
}
