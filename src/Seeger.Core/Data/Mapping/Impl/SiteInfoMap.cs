using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping.Impl
{
    class SiteInfoMap : ClassMapping<SiteInfo>
    {
        public SiteInfoMap()
        {
            Cache(c => c.Usage(CacheUsage.ReadWrite));

            Id(c => c.Culture, m =>
            {
                m.Generator(Generators.Assigned);
            });
            Lazy(false);

            Component(c => c.SEOInfo, m =>
            {
                m.Property(x => x.PageTitle, p => p.Column("SEO_PageTitle"));
                m.Property(x => x.MetaKeywords, p => p.Column("SEO_MetaKeywords"));
                m.Property(x => x.MetaDescription, p => p.Column("SEO_MetaDescription"));
            });
        }
    }
}
