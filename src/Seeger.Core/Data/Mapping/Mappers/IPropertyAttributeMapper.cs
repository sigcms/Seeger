using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    public interface IPropertyAttributeMapper
    {
        void ApplyMapping(Attribute attribute, PropertyPath property, IPropertyMapper mapper, MappingContext context);
    }
}
