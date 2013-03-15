using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Web;
using Seeger.Collections;
using NHibernate;
using NHibernate.Linq;

namespace Seeger.Caching
{
    public class PageCache
    {
        private ISession _session;

        public IEnumerable<PageItem> Pages
        {
            get
            {
                return _session.Query<PageItem>().OrderBy(x => x.Order).ThenBy(x => x.Id).Cacheable();
            }
        }

        public IEnumerable<PageItem> RootPages
        {
            get
            {
                return Pages.Where(x => x.Parent == null);
            }
        }

        private PageItem _homepage;

        public PageItem Homepage
        {
            get
            {
                if (_homepage == null)
                {
                    _homepage = RootPages.FirstOrDefault(it => it.Published);
                }
                return _homepage;
            }
        }

        public bool HasPage
        {
            get { return RootPages.Any(); }
        }

        private PageCache(ISession session)
        {
            _session = session;
        }

        public static PageCache From(ISession session)
        {
            return new PageCache(session);
        }

        public IEnumerable<PageItem> FindSiblings(int pageId)
        {
            var page = FindPage(pageId);
            if (page.Parent != null)
            {
                return page.Parent.Pages.Where(x => x.Id != pageId);
            }

            return RootPages.Where(x => x.Id != pageId);
        }

        public PageItem FindPage(int pageId)
        {
            return FindPage(p => p.Id == pageId);
        }

        public PageItem FindPage(Func<PageItem, bool> predicate)
        {
            Require.NotNull(predicate, "predicate");

            foreach (var page in RootPages)
            {
                if (predicate(page))
                {
                    return page;
                }

                var result = page.FindDescendant(predicate);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public PageItem FindPageByDomain(string domain)
        {
            return Pages.FirstOrDefault(x => x.HasBindedDomain(domain));
        }
    }
}
