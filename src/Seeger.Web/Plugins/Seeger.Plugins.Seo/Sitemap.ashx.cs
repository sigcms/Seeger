using Seeger.Plugins.Seo.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class _Sitemap : IHttpHandler
    {
        public void ProcessRequest(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/xml";

            var sitemapId = httpContext.Request.QueryString["sitemapId"];

            if (String.IsNullOrEmpty(sitemapId))
            {
                var context = GetSitemapContext(httpContext);
                var sitemaps = SitemapProviders.Providers.SelectMany(m => m.GetSitemaps(context)).ToList();
                if (sitemaps.Count == 1)
                {
                    RenderSitemap(sitemaps[0].Content, context, httpContext);
                }
                else if (sitemaps.Count > 1)
                {
                    // Render sitemap index
                    var entries = new List<SitemapIndexEntry>();
                    foreach (var sitemap in sitemaps)
                    {
                        entries.Add(new SitemapIndexEntry(String.Format("http://{0}/sitemap-{1}.xml", context.Domain, sitemap.Id)));
                    }

                    SitemapIndexSerializer.Serialize(entries).Save(httpContext.Response.Output);
                }
            }
            else
            {
                RenderSitemapPiece(httpContext, sitemapId);
            }
        }

        static void RenderSitemapPiece(HttpContext httpContext, string sitemapId)
        {
            foreach (var provider in SitemapProviders.Providers)
            {
                var context = GetSitemapContext(httpContext);
                var content = provider.GetContent(sitemapId, context);
                if (content != null)
                {
                    RenderSitemap(content, context, httpContext);
                    return;
                }
            }

            // If goes to here, then its a non-exist sitemapId. So we should throws 404
            throw new HttpException(404, "Page not found");
        }

        static void RenderSitemap(ISitemapContent content, SitemapContext context, HttpContext httpContext)
        {
            var urls = content.GetUrls(context);
            SitemapSerializer.Serialize(urls).Save(httpContext.Response.Output);
        }

        static SitemapContext GetSitemapContext(HttpContext httpContext)
        {
            return new SitemapContext(httpContext.Request.Url.Host);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}