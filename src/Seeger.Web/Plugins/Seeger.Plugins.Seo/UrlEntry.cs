using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class UrlEntry
    {
        public string Loc { get; set; }

        public DateTime Lastmod { get; set; }

        public ChangeFrequency Changefreq { get; set; }

        public float Priority { get; set; }

        public UrlEntry(string loc)
        {
            Loc = loc;
            Lastmod = DateTime.Today;
            Priority = 1.0f;
        }
    }
}