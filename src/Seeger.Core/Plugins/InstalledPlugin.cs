using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Plugins
{
    public class InstalledPlugin
    {
        public string PluginName { get; set; }

        public bool IsEnabled { get; set; }

        public InstalledPlugin(string pluginName, bool enabled)
        {
            Require.NotNullOrEmpty(pluginName, "pluginName");
            PluginName = pluginName;
            IsEnabled = enabled;
        }

        public string Serialize()
        {
            return PluginName + "," + IsEnabled;
        }

        public override string ToString()
        {
            return Serialize();
        }
    }
}
