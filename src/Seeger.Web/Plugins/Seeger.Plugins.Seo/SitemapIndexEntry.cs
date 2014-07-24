using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class SitemapIndexEntry
    {
        public string Loc { get; set; }

        public DateTime Lastmod { get; set; }

        public SitemapIndexEntry(string loc)
            : this(loc, DateTime.Today)
        {
        }

        public SitemapIndexEntry(string loc, DateTime lastmod)
        {
            Loc = loc;
            Lastmod = lastmod;
        }
    }
}