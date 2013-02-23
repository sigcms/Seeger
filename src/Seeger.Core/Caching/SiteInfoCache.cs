using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Data;
using NHibernate.Linq;
using NHibernate;

namespace Seeger.Caching
{
    public class SiteInfoCache
    {
        private ISession _session;

        public IEnumerable<SiteInfo> SiteInfos
        {
            get
            {
                return _session.Query<SiteInfo>().Cacheable();
            }
        }

        private SiteInfoCache(ISession session)
        {
            _session = session;
        }

        public static SiteInfoCache From(ISession session)
        {
            return new SiteInfoCache(session);
        }

        public SiteInfo GetSiteInfo(CultureInfo culture)
        {
            return SiteInfos.FirstOrDefault(x => x.Culture == culture.Name);
        }
    }
}
