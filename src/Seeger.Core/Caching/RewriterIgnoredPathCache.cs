using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Caching
{
    public class RewriterIgnoredPathCache
    {
        private ISession _session;

        public IEnumerable<RewriterIgnoredPath> RewriterIgnoredPaths
        {
            get
            {
                return _session.Query<RewriterIgnoredPath>().Cacheable();
            }
        }

        private RewriterIgnoredPathCache(ISession session)
        {
            _session = session;
        }

        public static RewriterIgnoredPathCache From(ISession session)
        {
            return new RewriterIgnoredPathCache(session);
        }
    }
}
