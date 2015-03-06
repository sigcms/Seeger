using Seeger.Caching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

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
                var matched = false;

                foreach (var candidate in candidates)
                {
                    if (candidate.UrlSegment.Equals(segments[i], StringComparison.OrdinalIgnoreCase))
                    {
                        matchedPage = candidate;
                        candidates = candidate.Pages;
                        context.RemainingSegments.RemoveAt(0);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    break;
                }
            }

            if (matchedPage != null)
            {
                context.MatchedPage = matchedPage;
            }

            if (context.MatchedPage != null)
            {
                // if RemainingSegments.Count > 0, it means this is not an exact match,
                // then we need to check if this is a directly file request.
                // Example: Cms page /home/products is binded to xxx.com using page domain binding.
                //          When a visitor visits website using xxx.com, page '/home/products' will always be matched.
                //          So if the visitor visits xxx.com/file.psd, we need to first check if file.psd exists in the file system.
                //          Otherwise the visitor will see homepage even when he is requesting file.psd
                if (context.RemainingSegments.Count > 0 && File.Exists(HostingEnvironment.MapPath(context.Request.Path)))
                {
                    context.MatchedPage = null;
                }
            }
        }
    }
}
