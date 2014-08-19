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
            var coreScriptBundle = new Bundle("~/Scripts/sig.core.js")
                                        .Include("~/Scripts/url.js")
                                        .Include("~/Scripts/knockout.js", "~/Scripts/knockout.mapping.js", "~/Scripts/knockout.bindings.js")
                                        .Include("~/Scripts/underscore.js", "~/Scripts/underscore.defaults.js")
                                        .Include("~/Scripts/jquery.validate.js", "~/Scripts/jquery.validate.unobtrusive.js", "~/Scripts/jquery.validate.unobtrusive.ext.js")
                                        .Include("~/Scripts/string.format.js")
                                        .Include("~/Scripts/uploadify/jquery.uploadify.js")
                                        .Include("~/Scripts/jquery.fileupload.js")
                                        .Include("~/Scripts/bootstrap.js")
                                        .IncludeDirectory("~/Scripts/sig", "*.js", true);

            BundleTable.Bundles.Add(coreScriptBundle);
        }
    }
}