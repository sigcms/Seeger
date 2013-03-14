using Seeger.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Seeger.Plugins
{
    public static class PluginPaths
    {
        public static readonly string ContainingDirectoryVirtualPath = "/Plugins";

        public static string PluginDirectoryVirtualPath(string pluginName)
        {
            return UrlUtil.Combine(ContainingDirectoryVirtualPath, pluginName);
        }

        public static string WidgetsFolderVirtualPath(string pluginName)
        {
            return UrlUtil.Combine(PluginDirectoryVirtualPath(pluginName), "Widgets");
        }
    }
}
