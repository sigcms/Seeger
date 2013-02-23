using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Data;
using Seeger.Caching;
using Seeger.Collections;
using NHibernate.Linq;

namespace Seeger.Services
{
    public class PageService
    {
        public static PageItemCollection GetRootPages()
        {
            var query = NhSessionManager.GetCurrentSession().Query<PageItem>().WhereIsRoot();
            return new PageItemCollection(null, query);
        }

        public static void Delete(int pageId, bool cascade)
        {
            var session = NhSessionManager.GetCurrentSession();

            var pageItem = session.Get<PageItem>(pageId);

            if (cascade)
            {
                List<int> toDeleteIds = new List<int>();

                pageItem.BreadthFirstVisit(true, it =>
                {
                    toDeleteIds.Add(it.Id);
                    return false;
                });

                var toDeletePages = PageCache.From(session).Pages.Where(it => toDeleteIds.Contains(it.Id)).ToList();
                for (var i = toDeleteIds.Count - 1; i >= 0; i--)
                {
                    var temp = toDeletePages.FirstOrDefault(it => it.Id == toDeleteIds[i]);
                    if (temp != null)
                    {
                        session.Delete(temp);
                    }
                }
            }
            else
            {
                session.Delete(pageItem);
            }

            session.Commit();
        }
    }
}
