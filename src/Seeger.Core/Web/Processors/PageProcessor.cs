using Seeger.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Processors
{
    public class PageProcessor : IHttpProcessor
    {
        public void Process(HttpProcessingContext context)
        {
            if (context.RemainingSegments.Count == 0) return;

            var checkUnpublished = false;

            Boolean.TryParse(context.Request.QueryString["showUnpublished"], out checkUnpublished);

            var segments = context.RemainingSegments.ToList();
            PageItem matchedPage = null;
            IEnumerable<PageItem> candidates = null;

            if (context.MatchedPage == null)
            {
                candidates = PageCache.From(context.NhSession).RootPages;
            }
            else
            {
                candidates = context.MatchedPage.Pages;
            }

            for (var i = 0; i < segments.Count; i++)
            {
                foreach (var candidate in candidates)
                {
                    if (candidate.UrlSegment.Equals(segments[i], StringComparison.OrdinalIgnoreCase))
                    {
                        matchedPage = candidate;
                        candidates = candidate.Pages;
                        context.RemainingSegments.RemoveAt(0);
                        break;
                    }
                }
            }

            if (matchedPage != null)
            {
                context.MatchedPage = matchedPage;
            }
        }
    }
}
