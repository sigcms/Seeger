using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping.Impl
{
    class PageItemMap : ClassMapping<PageItem>
    {
        public PageItemMap()
        {
            Property("_skinFullName", m =>
            {
                m.Column("SkinFullName");
                m.Access(Accessor.Field);
            });
            Property("_layoutFullName", m =>
            {
                m.Column("LayoutFullName");
                m.Access(Accessor.Field);
            });

            Property(c => c.Attributes, m =>
            {
                m.Type<EntityAttributeCollectionUserType>();
            });

            ManyToOne(c => c.Parent, m => m.Column("ParentPageId"));

            Bag<PageItem>(c => c.Pages, m =>
            {
                m.Key(k => k.Column("ParentPageId"));
                m.OrderBy("`Order`");
                m.Cache(c => c.Usage(CacheUsage.ReadWrite));
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
            }, m => m.OneToMany());

            Bag(c => c.LocatedWidgets, m => {
                m.Key(k => k.Column("PageId"));
                m.Inverse(true);
                m.OrderBy("`Order`");
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                m.Cache(c => c.Usage(CacheUsage.ReadWrite));
            }, m => m.OneToMany());
        }
    }
}
