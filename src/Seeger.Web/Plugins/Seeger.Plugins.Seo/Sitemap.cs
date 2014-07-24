using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class Sitemap
    {
        public string Id { get; set; }

        public ISitemapContent Content { get; set; }

        public Sitemap(string id, ISitemapContent content)
        {
            Id = id;
            Content = content;
        }
    }
}