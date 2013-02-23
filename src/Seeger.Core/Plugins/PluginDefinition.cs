using Seeger.Globalization;
using Seeger.Plugins.Widgets;
using Seeger.Security;
using Seeger.Web;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Seeger.Collections;
using System.Reflection;
using System.Web.Hosting;

namespace Seeger.Plugins
{
    public class PluginDefinition
    {
        public string Name { get; private set; }

        public LocalizableText DisplayName { get; set; }

        public LocalizableText Description { get; set; }

        public string Author { get; set; }

        public Type PluginType { get; set; }

        public string VirtualPath
        {
            get
            {
                return PluginPaths.PluginDirectoryVirtualPath(Name);
            }
        }

        public string ResourceFolderVirtualPath
        {
            get
            {
                return UrlUtility.Combine(VirtualPath, "Resources");
            }
        }

        public XmlMenu Menu { get; set; }

        public PermissionGroupCollection PermissionGroups { get; set; }

        public ResourcesFolder ResourcesFolder { get; private set; }

        public string Localize(string key, CultureInfo culture)
        {
            return Localize(key, culture, true);
        }

        public string Localize(string key, CultureInfo culture, bool searchUp)
        {
            var value = ResourcesFolder.GetValue(key, culture);
            if (value == null && searchUp)
            {
                value = ResourcesFolder.Global.GetValue(key, culture);
            }

            return value ?? key;
        }

        public IList<WidgetDefinition> Widgets { get; private set; }

        public PluginDefinition(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            Name = name;
            Widgets = new List<WidgetDefinition>();
            Menu = new XmlMenu();
            PermissionGroups = new PermissionGroupCollection();
            ResourcesFolder = new ResourcesFolder(HostingEnvironment.MapPath(ResourceFolderVirtualPath));
        }

        public WidgetDefinition FindWidget(string name)
        {
            Require.NotNullOrEmpty(name, "name");
            return Widgets.FirstOrDefault(it => it.Name == name);
        }

        public IPlugin CreatePluginInstance()
        {
            if (PluginType != null)
            {
                return (IPlugin)Activator.CreateInstance(PluginType);
            }

            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public static class PluginDefinitionCollectionExtensions
    {
        public static PluginDefinition FindByName(this IEnumerable<PluginDefinition> plugins, string pluginName)
        {
            return plugins.FirstOrDefault(x => x.Name == pluginName);
        }
    }
}
