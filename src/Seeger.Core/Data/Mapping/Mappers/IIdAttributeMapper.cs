using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    public interface IIdAttributeMapper
    {
        void ApplyMapping(Attribute attribute, MemberInfo idProperty, Type entityType, IClassAttributesMapper mapper, MappingContext context);
    }
}
