using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;

namespace Seeger.Data.Mapping.Mappers
{
    [MapperFor(typeof(IdAttribute))]
    public class IdAttributeMapper : IIdAttributeMapper
    {
        public void ApplyMapping(Attribute attribute, System.Reflection.MemberInfo idProperty, Type entityType, NHibernate.Mapping.ByCode.IClassAttributesMapper mapper, MappingContext context)
        {
            var idType = idProperty.GetPropertyOrFieldType();

            if (idType == typeof(Int32) || idType == typeof(Int64))
            {
                new HiloIdAttributeMapper().ApplyMapping(new HiloIdAttribute(), idProperty, entityType, mapper, context);
            }
            else if (idType == typeof(Guid))
            {
                new GuidAttributeMapper().ApplyMapping(new GuidAttribute(), idProperty, entityType, mapper, context);
            }
        }
    }
}
