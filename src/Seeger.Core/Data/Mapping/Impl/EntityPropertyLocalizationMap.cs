using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping.Impl
{
    class EntityPropertyLocalizationMap : ClassMapping<EntityPropertyLocalization>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(EntityPropertyLocalization).Name;
            }
        }

        public EntityPropertyLocalizationMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.EntityType);
            Property(c => c.EntityKey);
            Property(c => c.PropertyPath);
            Property(c => c.Culture);
            Property(c => c.PropertyValue, m => m.Type(NHibernateUtil.StringClob));
        }
    }
}
