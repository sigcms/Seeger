using NHibernate;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Mappers
{
    [MapperFor(typeof(StringClobAttribute))]
    public class StringClobAttributeMapper : IPropertyAttributeMapper
    {
        public void ApplyMapping(Attribute attribute, NHibernate.Mapping.ByCode.PropertyPath property, NHibernate.Mapping.ByCode.IPropertyMapper mapper, MappingContext context)
        {
            mapper.Type(NHibernateUtil.StringClob);
        }
    }
}
