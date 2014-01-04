using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping.Impl
{
    class FrontendLanguageMap : ClassMapping<FrontendLanguage>
    {
        public FrontendLanguageMap()
        {
            Table("cms_" + typeof(FrontendLanguage).Name);

            Cache(c => c.Usage(CacheUsage.ReadWrite));

            Id(c => c.Name, m => m.Generator(Generators.Assigned));
            Property(c => c.DisplayName);
            Property(c => c.BindedDomain);
        }
    }
}
