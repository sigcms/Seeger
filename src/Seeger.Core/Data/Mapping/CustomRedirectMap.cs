using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace Seeger.Data.Mapping
{
    class CustomRedirectMap : ClassMapping<CustomRedirect>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(CustomRedirect).Name;
            }
        }

        public CustomRedirectMap()
        {
            Table(TableName);

            Cache(c => c.Usage(CacheUsage.ReadWrite));

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.From, m => m.Column("`From`"));
            Property(c => c.To, m => m.Column("`To`"));
            Property(c => c.Description, m => m.Column("`Description`"));
            Property(c => c.Priority);
            Property(c => c.MatchByRegex);
            Property(c => c.RedirectMode);
            Property(c => c.IsEnabled);
            Property(c => c.UtcCreatedTime);
        }
    }
}
