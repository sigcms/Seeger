using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Globalization;
using System.Web;
using Seeger.Plugins;
using Seeger.Plugins.Widgets;

namespace Seeger.Web
{
    public class SmartLocalizer
    {
        private string _requestPath;
        private string _pluginName;
        private string _templateName;
        private string _widgetName;

        public string PluginName
        {
            get
            {
                EnsureParsed();
                return _pluginName;
            }
        }

        public string TemplateName
        {
            get
            {
                EnsureParsed();
                return _templateName;
            }
        }

        public string WidgetName
        {
            get
            {
                EnsureParsed();
                return _widgetName;
            }
        }

        public SmartLocalizer(string requestPath)
        {
            Require.NotNullOrEmpty(requestPath, "requestPath");

            _requestPath = requestPath;
        }

        public static SmartLocalizer GetForCurrentRequest()
        {
            var context = HttpContext.Current;
            const string key = "Seeger.SmartLocalizer.Current";

            var localizer = context.Items[key] as SmartLocalizer;

            if (localizer == null)
            {
                localizer = new SmartLocalizer(context.Request.Path);
                context.Items[key] = localizer;   
            }

            return localizer;
        }

        private bool _parsed;

        private void EnsureParsed()
        {
            if (!_parsed)
            {
                Parse();
                _parsed = true;
            }
        }

        private void Parse()
        {
            if (_requestPath == "/" || _requestPath.IgnoreCaseEquals("/Admin") || _requestPath.IgnoreCaseStartsWith("/Admin/")) return;

            var path = _requestPath;

            if (path.Length == 0 || path == "/" || _requestPath.IgnoreCaseEquals("/Admin") || _requestPath.IgnoreCaseStartsWith("/Admin/")) return;

            var segments = path.SplitWithoutEmptyEntries('/');

            if (segments[0].IgnoreCaseEquals("Plugins"))
            {
                if (segments.Length > 1)
                {
                    _pluginName = segments[1];
                }
                else
                {
                    return;
                }
            }
            else if (segments[0].IgnoreCaseEquals("Templates"))
            {
                if (segments.Length > 1)
                {
                    _templateName = segments[1];
                }
                else
                {
                    return;
                }
            }

            if (segments.Length > 2)
            {
                if (segments[2].IgnoreCaseEquals("Widgets"))
                {
                    _widgetName = segments[2];
                }
            }
        }

        public string Localize(string key, CultureInfo culture)
        {
            Require.NotNullOrEmpty(key, "key");
            Require.NotNull(culture, "culture");

            PluginDefinition plugin = null;

            if (!String.IsNullOrEmpty(PluginName))
            {
                plugin = PluginManager.FindEnabledPlugin(PluginName);
            }

            if (plugin != null)
            {
                WidgetDefinition widget = null;

                if (!String.IsNullOrEmpty(WidgetName))
                {
                    widget = plugin.FindWidget(WidgetName);
                }

                if (widget != null)
                {
                    return widget.Localize(key, culture, true);
                }

                return plugin.Localize(key, culture, true);
            }

            var value = ResourcesFolder.Global.GetValue(key, culture);

            return value ?? key;
        }
    }
}
