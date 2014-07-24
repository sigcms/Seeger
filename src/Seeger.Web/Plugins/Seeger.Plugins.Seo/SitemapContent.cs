using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class SitemapContent : ISitemapContent
    {
        private IEnumerable<UrlEntry> _urls;

        public SitemapContent(IEnumerable<UrlEntry> urls)
        {
            _urls = urls;
        }

        public IEnumerable<UrlEntry> GetUrls(SitemapContext context)
        {
            return _urls;
        }
    }
}