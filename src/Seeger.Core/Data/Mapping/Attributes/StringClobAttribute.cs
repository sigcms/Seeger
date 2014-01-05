using NHibernate;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StringClobAttribute : PropertyAttribute
    {
        public override void ApplyMapping(IModelInspector modelInspector, PropertyPath member, IPropertyMapper mapper)
        {
            mapper.Type(NHibernateUtil.StringClob);
        }
    }
}
