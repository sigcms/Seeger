using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace Seeger.Data.Mapping.Impl
{
    class CustomRedirectMap : ClassMapping<CustomRedirect>
    {
        public CustomRedirectMap()
        {
            Cache(c => c.Usage(CacheUsage.ReadWrite));
        }
    }
}
