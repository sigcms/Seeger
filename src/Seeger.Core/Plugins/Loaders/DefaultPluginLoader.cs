using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Hosting;
using System.IO;
using Seeger.Plugins.Widgets.Loaders;
using Seeger.Web;
using System.Xml.Linq;
using Seeger.Globalization;
using Seeger.Web.UI;
using Seeger.Collections;
using Seeger.Security;
using Seeger.Data;
using Seeger.Plugins.Widgets;

namespace Seeger.Plugins.Loaders
{
    public class DefaultPluginLoader : IPluginLoader
    {
        private IWidgetLoader _widgetLoader;
        static readonly string[] _widgetControllerSuffixes = new[] { "WidgetController", "Controller" };

        public DefaultPluginLoader(IWidgetLoader widgetLoader)
        {
            _widgetLoader = widgetLoader;
        }

        public PluginDefinition Load(string pluginName, IEnumerable<Assembly> assemblies)
        {
            var plugin = new PluginDefinition(pluginName);

            Configure(plugin);

            if (assemblies != null)
            {
                plugin.Assemblies = assemblies.ToList();

                var extensionTypes = FindExtensionTypes(assemblies);
                plugin.PluginType = extensionTypes.PluginType;

                if (extensionTypes.NhMappingProviderType != null)
                    NhMappingProviders.Register(pluginName, extensionTypes.NhMappingProviderType);

                if (extensionTypes.PageLifecycleInterceptorTypes.Count > 0)
                {
                    PageLifecycleInterceptors.Register(pluginName,
                        extensionTypes.PageLifecycleInterceptorTypes.Select(t => (IPageLifecycleInterceptor)Activator.CreateInstance(t)));
                }
            }

            var widgetBaseFolderPath = HostingEnvironment.MapPath(PluginPaths.WidgetsFolderVirtualPath(pluginName));

            if (Directory.Exists(widgetBaseFolderPath))
            {
                foreach (var widgetFolder in Directory.GetDirectories(widgetBaseFolderPath))
                {
                    var widget = _widgetLoader.LoadWidget(plugin, Path.GetFileName(widgetFolder));
                    plugin.Widgets.Add(widget);
                }
            }

            // load widget controllers
            if (assemblies != null)
            {
                foreach (var type in assemblies.SelectMany(asm => asm.GetTypes()))
                {
                    if (!typeof(IWidgetController).IsAssignableFrom(type))
                    {
                        continue;
                    }

                    var attr = type.GetCustomAttribute<WidgetControllerAttribute>();
                    if (attr == null)
                    {
                        continue;
                    }

                    var widgetName = attr.WidgetName;
                    if (String.IsNullOrEmpty(widgetName))
                    {
                        widgetName = type.Name;
                        foreach (var suffix in _widgetControllerSuffixes)
                        {
                            if (widgetName.EndsWith(suffix) && widgetName.Length > suffix.Length)
                            {
                                widgetName = widgetName.Substring(0, widgetName.Length - suffix.Length);
                                break;
                            }
                        }
                    }

                    var widget = plugin.FindWidget(widgetName);
                    if (widget != null)
                    {
                        widget.WidgetControllerType = type;
                    }
                }
            }

            return plugin;
        }

        private void Configure(PluginDefinition plugin)
        {
            string configFilePath = Server.MapPath(UrlUtil.Combine(plugin.VirtualPath, "config.config"));
            if (File.Exists(configFilePath))
            {
                var xml = XDocument.Load(configFilePath).Root;

                var displayName = xml.ChildElementValue("display-name");
                if (!String.IsNullOrEmpty(displayName))
                {
                    plugin.DisplayName = new LocalizableText(displayName);
                    plugin.DisplayName.ResourceDescriptor.PluginName = plugin.Name;
                }

                var description = xml.ChildElementValue("description");
                if (!String.IsNullOrEmpty(description))
                {
                    plugin.Description = new LocalizableText(description);
                    plugin.Description.ResourceDescriptor.PluginName = plugin.Name;
                }

                var managementElement = xml.Element("management");
                if (managementElement != null)
                {
                    ConfigureManagement(plugin, managementElement);
                }

                var permissionsElement = xml.Element("permissions");
                if (permissionsElement != null)
                {
                    ConfigurePermissions(plugin, permissionsElement);
                }
            }
        }

        private void ConfigureManagement(PluginDefinition plugin, XElement xml)
        {
            var menuElement = xml.Element("management-menu");
            if (menuElement != null)
            {
                var menu = XmlMenuLoader.LoadMenu(menuElement);
                RecursivelyAdjustMenuItems(menu.Items, plugin);
                plugin.Menu = menu;
            }
        }

        private void RecursivelyAdjustMenuItems(IEnumerable<XmlMenuItem> items, PluginDefinition plugin)
        {
            foreach (var item in items)
            {
                if (item.NavigateUrl != null && !item.NavigateUrl.StartsWith("/"))
                {
                    item.NavigateUrl = UrlUtil.ToAbsoluteHtmlPath(UrlUtil.Combine(plugin.VirtualPath, item.NavigateUrl));
                }

                item.Title.ResourceDescriptor.PluginName = plugin.Name;

                if (String.IsNullOrEmpty(item.Plugin))
                {
                    item.Plugin = plugin.Name;
                }

                RecursivelyAdjustMenuItems(item.Items, plugin);
            }
        }

        private void ConfigurePermissions(PluginDefinition plugin, XElement xml)
        {
            var groups = new PermissionGroupCollection();

            foreach (var element in xml.Elements())
            {
                var group = PermissionGroup.From(element, plugin);
                groups.Add(group);
            }

            plugin.PermissionGroups = groups;
        }

        private ExtensionTypes FindExtensionTypes(IEnumerable<Assembly> assemblies)
        {
            var result = new ExtensionTypes();

            foreach (var type in assemblies.SelectMany(it => it.GetTypes()))
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (typeof(IPlugin).IsAssignableFrom(type))
                    {
                        result.PluginType = type;
                    }
                    if (typeof(INhMappingProvider).IsAssignableFrom(type))
                    {
                        result.NhMappingProviderType = type;
                    }
                    if (typeof(IPageLifecycleInterceptor).IsAssignableFrom(type))
                    {
                        result.PageLifecycleInterceptorTypes.Add(type);
                    }
                }
            }

            return result;
        }

        class ExtensionTypes
        {
            public Type PluginType { get; set; }

            public Type NhMappingProviderType { get; set; }

            public IList<Type> PageLifecycleInterceptorTypes { get; set; }

            public ExtensionTypes()
            {
                PageLifecycleInterceptorTypes = new List<Type>();
            }
        }
    }
}
