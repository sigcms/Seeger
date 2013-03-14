using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Seeger.Web;

namespace Seeger.Plugins.Widgets
{
    public static class WidgetPaths
    {
        public static string WidgetDirectoryVirtualPath(string pluginName, string widgetName)
        {
            return UrlUtil.Combine(PluginPaths.WidgetsFolderVirtualPath(pluginName), widgetName);
        }

        public static string WidgetResourcesDirectoryVirtualPath(string pluginName, string widgetName)
        {
            return UrlUtil.Combine(WidgetDirectoryVirtualPath(pluginName, widgetName), "Resources");
        }
    }
}
