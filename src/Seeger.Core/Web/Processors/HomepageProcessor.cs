using Seeger.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Processors
{
    public class HomepageProcessor : IHttpProcessor
    {
        public void Process(HttpProcessingContext context)
        {
            if (context.MatchedPage != null) return;

            if (context.RemainingSegments.Count == 0 || context.RemainingSegments[0].Equals("default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                var pageCache = PageCache.From(context.NhSession);
                if (pageCache.Homepage != null && pageCache.Homepage.Published)
                {
                    context.MatchedPage = pageCache.Homepage;
                    context.StopProcessing = true;

                    if (context.RemainingSegments.Count > 0)
                    {
                        context.RemainingSegments.RemoveAt(0);
                    }

                    return;
                }

                throw new HttpException(404, "Page Not Found.");
            }
        }
    }
}
