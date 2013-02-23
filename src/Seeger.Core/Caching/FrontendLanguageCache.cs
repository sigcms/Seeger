using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Caching
{
    public class FrontendLanguageCache
    {
        private ISession _session;

        public IEnumerable<FrontendLanguage> Languages
        {
            get
            {
                return _session.Query<FrontendLanguage>().Cacheable();
            }
        }

        private FrontendLanguageCache(ISession session)
        {
            _session = session;
        }

        public static FrontendLanguageCache From(ISession session)
        {
            return new FrontendLanguageCache(session);
        }

        public FrontendLanguage FindByDomain(string domain)
        {
            return Languages.FirstOrDefault(x => x.BindedDomain == domain);
        }

        public FrontendLanguage FindByName(string name)
        {
            return Languages.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool Contains(string name)
        {
            return Languages.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
