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
            if (pageId <= 0)
            {
                return null;
            }

            PageItem result = null;

            foreach (var page in RootPages)
            {
                if (page.Id.Equals(pageId))
                {
                    result = page;
                    break;
                }
                result = page.FindDecendant(it => it.Id.Equals(pageId));
                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public PageItem FindPage(Func<PageItem, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            PageItem result = null;
            foreach (var page in RootPages)
            {
                result = page.DepthFirstSearch(true, predicate);
                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public PageItem FindPageByDomain(string domain)
        {
            return Pages.FirstOrDefault(x => x.HasBindedDomain(domain));
        }
    }
}
