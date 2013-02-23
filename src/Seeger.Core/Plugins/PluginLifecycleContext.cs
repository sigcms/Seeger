using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Plugins
{
    public class PluginLifecycleContext
    {
        public PluginDefinition Plugin { get; private set; }

        public PluginLifecycleContext(PluginDefinition plugin)
        {
            Plugin = plugin;
        }
    }
}
