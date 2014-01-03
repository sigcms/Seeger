using NHibernate;
using NHibernate.Mapping.ByCode;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping
{
    class ConventionModelMapper : NHibernate.Mapping.ByCode.ConventionModelMapper
    {
        public string TablePrefix { get; private set; }

        public ConventionModelMapper()
            : this(null)
        {
        }

        public ConventionModelMapper(string tablePrefix)
        {
            TablePrefix = tablePrefix;
            BeforeMapClass += ConventionModelMapper_BeforeMapClass;
            BeforeMapProperty += ConventionModelMapper_BeforeMapProperty;
        }

        void ConventionModelMapper_BeforeMapClass(NHibernate.Mapping.ByCode.IModelInspector modelInspector, Type type, NHibernate.Mapping.ByCode.IClassAttributesMapper classCustomizer)
        {
            var tableName = GetTableName(type);

            // Table name
            classCustomizer.Table(tableName);

            // Hilo Id
            var member = MembersProvider.GetEntityMembersForPoid(type).FirstOrDefault(m => modelInspector.IsPersistentId(m));
            var hiloAttr = member.GetCustomAttributes(typeof(HiloIdAttribute), true)
                                 .OfType<HiloIdAttribute>()
                                 .FirstOrDefault();

            if (hiloAttr != null)
            {
                classCustomizer.Id(IdMappings.HighLowId(tableName));
            }
        }

        void ConventionModelMapper_BeforeMapProperty(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            if (member.LocalMember.GetPropertyOrFieldType() == typeof(String))
            {
                if (member.LocalMember.GetCustomAttributes(typeof(StringClobAttribute), false).Any())
                {
                    propertyCustomizer.Type(NHibernateUtil.StringClob);
                }
            }
        }

        private string GetTableName(Type type)
        {
            var prefix = TablePrefix;

            if (String.IsNullOrEmpty(prefix))
            {
                return type.Name;
            }

            if (!prefix.EndsWith("_"))
            {
                prefix = prefix + "_";
            }

            return prefix + type.Name;
        }
    }
}
