using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace Seeger.Data.Mapping
{
    class RewriterIgnoredPathMap : ClassMapping<RewriterIgnoredPath>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(RewriterIgnoredPath).Name;
            }
        }

        public RewriterIgnoredPathMap()
        {
            Table(TableName);

            Cache(c => c.Usage(CacheUsage.ReadWrite));

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Name);
            Property(c => c.Path);
            Property(c => c.MatchByRegex);
            Property(c => c.Reserved);
        }
    }
}
