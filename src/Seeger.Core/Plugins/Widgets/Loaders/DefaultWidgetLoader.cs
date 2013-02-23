using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;
using Seeger.Web;
using System.Xml.Linq;
using Seeger.Globalization;
using Seeger.Web.UI;

namespace Seeger.Plugins.Widgets.Loaders
{
    public class DefaultWidgetLoader : IWidgetLoader
    {
        public WidgetDefinition LoadWidget(PluginDefinition plugin, string widgetName)
        {
            var directory = HostingEnvironment.MapPath(WidgetPaths.WidgetDirectoryVirtualPath(plugin.Name, widgetName));
            var widget = new WidgetDefinition(Path.GetFileName(directory), plugin);
            Configure(widget);
            return widget;
        }

        private void Configure(WidgetDefinition widget)
        {
            var files = new DirectoryInfo(Server.MapPath(widget.VirtualPath)).GetFiles();

            var iconFile = files.FirstOrDefault(f => f.Name.IgnoreCaseStartsWith("icon."));
            if (iconFile != null)
            {
                widget.IconUrl = UrlUtility.Combine(widget.VirtualPath, iconFile.Name);
            }

            widget.Editable = files.Any(f => f.Name.IgnoreCaseEquals("Editor.aspx"));

            var configFile = files.FirstOrDefault(f => f.Name.IgnoreCaseEquals("config.config"));
            if (configFile != null)
            {
                ApplyXmlConfig(widget, XDocument.Load(configFile.FullName).Root);
            }

            if (widget.DisplayName == null)
            {
                widget.DisplayName = new LocalizableText(String.Format("{{ Plugin={0}, Key=WidgetName.{1} }}", widget.Plugin.Name, widget.Name));
            }
        }

        private void ApplyXmlConfig(WidgetDefinition widget, XElement xml)
        {
            var displayName = xml.ChildElementValue("display-name");
            if (!String.IsNullOrEmpty(displayName))
            {
                widget.DisplayName = new LocalizableText(displayName);
            }

            var category = xml.ChildElementValue("category");
            if (!String.IsNullOrEmpty(category))
            {
                widget.Category = new LocalizableText(category);
                widget.Category.ResourceDescriptor.PluginName = widget.Plugin.Name;
                widget.Category.ResourceDescriptor.WidgetName = widget.Name;
            }

            var iconUrl = xml.ChildElementValue("icon-url");
            if (!String.IsNullOrEmpty(iconUrl))
            {
                widget.IconUrl = iconUrl;
            }

            var widgetProcessEventListenerType = xml.ChildElementValue("process-event-listener");
            if (!String.IsNullOrEmpty(widgetProcessEventListenerType))
            {
                var processor = (IWidgetProcessEventListener)Activator.CreateInstance(Type.GetType(widgetProcessEventListenerType, true));
                widget.WidgetProcessEventListener = processor;
            }

            var editorElement = xml.Element("editor");
            if (editorElement != null)
            {
                widget.EditorSettings = WidgetEditorSettings.From(editorElement);
            }
        }
    }
}
