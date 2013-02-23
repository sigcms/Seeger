using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping
{
    class WidgetInPageMap : ClassMapping<WidgetInPage>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(WidgetInPage).Name;
            }
        }

        public WidgetInPageMap()
        {
            Table(TableName);

            Cache(c => c.Usage(CacheUsage.ReadWrite));

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.PluginName);
            Property(c => c.ZoneName);
            Property(c => c.WidgetName);
            Property(c => c.Attributes, m =>
            {
                m.Type<EntityAttributeCollectionUserType>();
            });
            Property(c => c.Order, m => m.Column("`Order`"));

            ManyToOne<PageItem>(c => c.Page, m =>
            {
                m.Column("PageId");
            });
        }
    }
}
