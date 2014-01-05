using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Data.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IdAttribute : Attribute
    {
        public virtual void ApplyMapping(IModelInspector modelInspector, Type entityType, string tableName, MemberInfo property, IClassAttributesMapper mapper)
        {
            var idType = property.GetPropertyOrFieldType();
            IdAttribute defaultMappingAttribute = null;

            if (idType == typeof(Int32) || idType == typeof(Int64))
            {
                defaultMappingAttribute = new HiloIdAttribute();
            }
            else if (idType == typeof(Guid))
            {
                defaultMappingAttribute = new GuidAttribute();
            }

            if (defaultMappingAttribute != null)
            {
                defaultMappingAttribute.ApplyMapping(modelInspector, entityType, tableName, property, mapper);
            }
        }
    }
}
