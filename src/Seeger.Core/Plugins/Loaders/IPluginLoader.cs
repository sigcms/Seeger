using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Seeger.Plugins.Widgets.Loaders;

namespace Seeger.Plugins.Loaders
{
    public interface IPluginLoader
    {
        PluginDefinition Load(string pluginName, IEnumerable<Assembly> assemblies);
    }

    public static class PluginLoaders
    {
        public static Func<IPluginLoader> Current = () => new DefaultPluginLoader(new DefaultWidgetLoader());
    }
}
