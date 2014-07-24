using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class SitemapContext
    {
        public string Domain { get; private set; }

        public SitemapContext(string domain)
        {
            Domain = domain;
        }
    }
}