using Seeger.Plugins.Seo.Processors;
using Seeger.Web.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class SeoPlugin : PluginBase
    {
        public override void OnStartup(PluginLifecycleContext context)
        {
            HttpProcessors.Processors.Insert(0, SitemapProcessor.Instance);
        }

        public override void OnDisable(PluginLifecycleContext context)
        {
            HttpProcessors.Processors.Remove(SitemapProcessor.Instance);
        }
    }
}