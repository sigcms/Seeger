using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Seeger.Plugins.Seo.Serialization
{
    public static class SitemapIndexSerializer
    {
        public static XDocument Serialize(IEnumerable<SitemapIndexEntry> sitemaps)
        {
            var doc = new XDocument(new XElement("sitemapindex"));

            foreach (var sitemap in sitemaps)
            {
                doc.Root.Add(Serialize(sitemap));
            }

            return doc;
        }

        static XElement Serialize(SitemapIndexEntry sitemap)
        {
            return new XElement("sitemap",
                new XElement("loc", sitemap.Loc),
                new XElement("lastmod", sitemap.Lastmod.ToString("yyyy-MM-dd"))
            );
        }
    }
}