using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public static class SitemapProviders
    {
        static readonly SitemapProviderCollection _providers = new SitemapProviderCollection();

        public static SitemapProviderCollection Providers
        {
            get
            {
                return _providers;
            }
        }
    }
}