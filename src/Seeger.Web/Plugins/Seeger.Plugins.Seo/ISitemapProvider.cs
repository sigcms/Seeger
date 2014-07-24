using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public interface ISitemapProvider
    {
        IEnumerable<Sitemap> GetSitemaps(SitemapContext context);

        ISitemapContent GetContent(string sitemapId, SitemapContext context);
    }
}