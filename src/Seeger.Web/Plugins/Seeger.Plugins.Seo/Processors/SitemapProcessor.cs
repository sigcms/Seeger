using Seeger.Web.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Seeger.Plugins.Seo.Processors
{
    public class SitemapProcessor : IHttpProcessor
    {
        public static readonly SitemapProcessor Instance = new SitemapProcessor();

        static readonly Regex _regex = new Regex(@"^sitemap(\-(?<id>[\w\-]+))?\.xml$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void Process(HttpProcessingContext context)
        {
            if (context.RemainingSegments.Count == 0)
            {
                return;
            }

            var match = _regex.Match(context.RemainingSegments[context.RemainingSegments.Count - 1]);
            if (match.Success)
            {
                var rewritePath = "~/Plugins/Seeger.Plugins.Seo/Sitemap.ashx";
                var idGroup = match.Groups["id"];
                if (idGroup.Success)
                {
                    rewritePath += "?sitemapId=" + idGroup.Value;
                }

                context.HttpContext.RewritePath(rewritePath);
                context.StopProcessing = true;
            }
        }
    }
}