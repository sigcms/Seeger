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
        public string TableName
        {
            get
            {
                return "cms_" + typeof(PageItem).Name;
            }
        }

        public PageItemMap()
        {
            Table(TableName);

            Cache(c => c.Usage(CacheUsage.ReadWrite));

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.UniqueName);
            Property(c => c.DisplayName);
            Property(c => c.UrlSegment);
            Property(c => c.BindedDomains);
            Property(c => c.Order, m => m.Column("`Order`"));
            Property(c => c.CreatedTime);
            Property(c => c.ModifiedTime);
            Property(c => c.VisibleInMenu);
            Property(c => c.Published);
            Property(c => c.IsDeletable);
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
            Property(c => c.MenuText);
            Property(c => c.PageTitle);
            Property(c => c.MetaKeywords);
            Property(c => c.MetaDescription);
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
