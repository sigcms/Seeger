using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.Web;

namespace Seeger.Caching
{
    public class CustomRedirectCache
    {
        private ISession _session;

        public IEnumerable<CustomRedirect> CustomRedirects
        {
            get
            {
                return _session.Query<CustomRedirect>().OrderBy(x => x.Id).Cacheable();
            }
        }

        private CustomRedirectCache(ISession session)
        {
            _session = session;
        }

        public static CustomRedirectCache From(ISession session)
        {
            return new CustomRedirectCache(session);
        }

        public CustomRedirect Match(HttpRequest request)
        {
            return CustomRedirects.FirstOrDefault(x => x.IsMatch(request));
        }
    }
}
