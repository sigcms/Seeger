using Newtonsoft.Json.Linq;
using Seeger.Plugins;
using Seeger.Plugins.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Seeger.Web.UI
{
    public class LocatedWidgetViewModel
    {
        public int Id { get; set; }

        public string WidgetName { get; set; }

        public string PluginName { get; set; }

        public string ZoneName { get; set; }

        public WidgetState State { get; set; }

        public int Order { get; set; }

        public IDictionary<string, string> Attributes { get; private set; }

        public JObject CustomData { get; set; }

        public LocatedWidgetViewModel()
        {
            Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }

}
