using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class PageCollectionExtensions
    {
        public static IQueryable<PageItem> WhereIsRoot(this IQueryable<PageItem> src)
        {
            return src.Where(it => it.Parent == null);
        }

        public static PageItem FindByUrlSegment(this IEnumerable<PageItem> pages, string urlSegment)
        {
            return pages.FirstOrDefault(p => p.UrlSegment.Equals(urlSegment, StringComparison.OrdinalIgnoreCase));
        }

        public static PageItem FindByUniqueName(this IEnumerable<PageItem> pages, string uniqueName)
        {
            return pages.FirstOrDefault(p => p.UniqueName == uniqueName);
        }

        public static void AdjustOrders(this IEnumerable<PageItem> pages, bool recursive)
        {
            PageItem last = null;

            foreach (var page in pages)
            {
                if (last == null)
                {
                    last = page;
                }
                else if (last.Order >= page.Order)
                {
                    page.Order = last.Order + 1;
                }

                if (recursive)
                {
                    page.Pages.AdjustOrders(recursive);
                }
            }
        }
    }
}
