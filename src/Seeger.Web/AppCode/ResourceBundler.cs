using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Seeger.Web
{
    public static class ResourceBundler
    {
        public static void Initialize()
        {
            BundleTable.Bundles.Add(new Bundle("~/Scripts/sig.core.js").IncludeDirectory("~/Scripts/sig", "*.js", true));
        }
    }
}