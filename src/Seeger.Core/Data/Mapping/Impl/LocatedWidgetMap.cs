using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping.Impl
{
    class LocatedWidgetMap : ClassMapping<LocatedWidget>
    {
        public LocatedWidgetMap()
        {
            Cache(c => c.Usage(CacheUsage.ReadWrite));

            Property(c => c.Attributes, m =>
            {
                m.Type<EntityAttributeCollectionUserType>();
            });

            ManyToOne<PageItem>(c => c.Page, m =>
            {
                m.Column("PageId");
            });
        }
    }
}
