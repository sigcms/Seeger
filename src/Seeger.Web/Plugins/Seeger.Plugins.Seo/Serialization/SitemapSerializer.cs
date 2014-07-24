using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Seeger.Plugins.Seo.Serialization
{
    public static class SitemapSerializer
    {
        public static XDocument Serialize(IEnumerable<UrlEntry> urls)
        {
            var doc = new XDocument(new XElement("urlset"));

            foreach (var url in urls)
            {
                doc.Root.Add(Serialize(url));
            }

            return doc;
        }

        static XElement Serialize(UrlEntry url)
        {
            return new XElement("url",
                new XElement("loc", url.Loc),
                new XElement("lastmod", url.Lastmod.ToString("yyyy-MM-dd")),
                new XElement("changefreq", url.Changefreq.ToString().ToLower()),
                new XElement("priority", url.Priority.ToString("f1"))
            );
        }
    }
}