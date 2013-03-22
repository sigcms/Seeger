using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Plugins;
using Seeger.Templates;

namespace Seeger.Globalization
{
    /// <summary>
    /// A class to parse string representations of resource descriptors.
    /// </summary>
    /// <remarks>
    /// Resource Descriptor Syntax:
    /// <code>
    /// { Plugin=...; Template=...; Widget=...; Key=... }
    /// </code>
    /// The '{' and '}' is optional. The 'Plugin', 'Template' and 'Widget' parameters are optional as well.
    /// If the descriptor passed in does not conform to this syntax, it will be treated as the resource key.
    /// </remarks>
    public class ResourceDescriptor
    {
        private static readonly string[] _paramSperators = new string[] { ";", "," };

        private string _descriptor;

        public string ResourceKey { get; internal set; }
        public string PluginName { get; internal set; }
        public string TemplateName { get; internal set; }
        public string WidgetName { get; internal set; }

        public bool IsValid
        {
            get { return !String.IsNullOrEmpty(ResourceKey); }
        }

        private ResourceDescriptor(string descriptor)
        {
            if (descriptor == null)
            {
                _descriptor = String.Empty;
            }
            else
            {
                _descriptor = descriptor.Trim();
            }

            Parse();
        }

        #region Parsing

        private void Parse()
        {
            if (_descriptor.Length == 0) return;

            if (_descriptor.Length > 2 && _descriptor.StartsWith("{") && _descriptor.EndsWith("}"))
            {
                ParseBody(_descriptor.Substring(1, _descriptor.Length - 2));
            }
            else
            {
                ParseBody(_descriptor);
            }
        }

        private void ParseBody(string body)
        {
            string[] assignments = body.Split(_paramSperators, StringSplitOptions.RemoveEmptyEntries);

            if (assignments.Length == 1)
            {
                ResourceKey = assignments[0].Trim();
            }
            else
            {
                foreach (var each in assignments)
                {
                    ParseAssignment(each);
                }

                if (String.IsNullOrEmpty(ResourceKey))
                    throw new InvalidOperationException("'Key' parameter is required in the resource descriptor string.");
            }
        }

        private void ParseAssignment(string assignment)
        {
            string[] parts = assignment.Split('=');

            if (parts.Length == 2)
            {
                string param = parts[0].Trim();
                string value = parts[1].Trim();

                if (param.IgnoreCaseEquals("Plugin"))
                {
                    PluginName = value;
                }
                else if (param.IgnoreCaseEquals("Template"))
                {
                    TemplateName = value;
                }
                else if (param.IgnoreCaseEquals("Widget"))
                {
                    WidgetName = value;
                }
                else if (param.IgnoreCaseEquals("Key"))
                {
                    ResourceKey = value;
                }
            }
        }

        #endregion

        public static ResourceDescriptor Parse(string descriptor)
        {
            return new ResourceDescriptor(descriptor);
        }

        public string GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");

            if (!String.IsNullOrEmpty(PluginName))
            {
                builder.Append("Plugin=").Append(PluginName).Append("; ");
            }
            if (!String.IsNullOrEmpty(TemplateName))
            {
                builder.Append("Template=").Append(TemplateName).Append("; ");
            }
            if (!String.IsNullOrEmpty(WidgetName))
            {
                builder.Append("Widget=").Append(WidgetName).Append("; ");
            }
            if (!String.IsNullOrEmpty(ResourceKey))
            {
                builder.Append("Key=").Append(ResourceKey);
            }

            builder.Append(" }");

            return builder.ToString();
        }

        public override string ToString()
        {
            return GetStringRepresentation();
        }

        public string Localize(CultureInfo culture)
        {
            if (!IsValid)
            {
                return _descriptor;
            }

            if (culture == null)
            {
                culture = CultureInfo.CurrentUICulture;
            }

            PluginDefinition plugin = null;

            if (!String.IsNullOrEmpty(PluginName))
            {
                plugin = PluginManager.FindLoadedPlugin(PluginName);
            }

            if (plugin != null && !String.IsNullOrEmpty(WidgetName))
            {
                var widget = plugin.FindWidget(WidgetName);

                if (widget != null)
                {
                    return widget.Localize(ResourceKey, culture, true);
                }
            }

            if (plugin != null)
            {
                return plugin.Localize(ResourceKey, culture, true);
            }

            if (!String.IsNullOrEmpty(TemplateName))
            {
                var template = TemplateManager.FindTemplate(TemplateName);
                if (template != null)
                {
                    return template.Localize(ResourceKey, culture, true);
                }
            }

            var value = ResourceFolder.Global.GetValue(ResourceKey, culture);

            return value ?? ResourceKey;
        }
    }
}
