using Seeger.Globalization;
using Seeger.Web;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Seeger.Plugins.Widgets
{
    public class WidgetDefinition
    {
        public string Name { get; set; }

        public LocalizableText DisplayName { get; set; }

        public LocalizableText Category { get; set; }

        public string IconUrl { get; set; }

        public bool Editable { get; set; }

        public string Description { get; set; }

        public WidgetEditorSettings EditorSettings { get; set; }

        public Type InterceptorType { get; set; }

        public string VirtualPath
        {
            get
            {
                return WidgetPaths.WidgetDirectoryVirtualPath(Plugin.Name, Name);
            }
        }

        public string DesignerVirtualPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Designer.ascx");
            }
        }

        public string WidgetControlVirtualPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Default.ascx");
            }
        }

        public string ResourceFolderVirtualPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Resources");
            }
        }

        public PluginDefinition Plugin { get; private set; }

        public IWidgetProcessEventListener WidgetProcessEventListener { get; set; }

        public ResourceFolder ResourcesFolder { get; private set; }

        public WidgetDefinition(string name, PluginDefinition plugin)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(plugin, "plugin");

            Name = name;
            Plugin = plugin;
            EditorSettings = new WidgetEditorSettings();
            ResourcesFolder = new ResourceFolder(HostingEnvironment.MapPath(ResourceFolderVirtualPath));
        }

        public string Localize(string key, CultureInfo culture, bool searchUp)
        {
            var value = ResourcesFolder.GetValue(key, culture);
            if (value != null)
            {
                return value;
            }

            if (searchUp)
            {
                return Plugin.Localize(key, culture, true);
            }

            return key;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
