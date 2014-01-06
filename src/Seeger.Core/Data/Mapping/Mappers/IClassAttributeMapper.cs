using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    public interface IClassAttributeMapper
    {
        void ApplyMapping(Attribute attribute, Type type, IClassAttributesMapper mapper, MappingContext context);
    }
}
