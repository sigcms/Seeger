using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Plugins
{
    class LoadedPlugin
    {
        public bool IsInstalled { get; private set; }

        public bool IsEnabled { get; private set; }

        public PluginDefinition PluginDefinition { get; private set; }

        public IEnumerable<Assembly> Assemblies { get; private set; }

        public LoadedPlugin(PluginDefinition pluginDefinition, IEnumerable<Assembly> assemblies)
        {
            Require.NotNull(pluginDefinition, "pluginDefinition");
            Require.NotNull(assemblies, "assemblies");

            Assemblies = assemblies;
            PluginDefinition = pluginDefinition;
        }

        public void MarkInstalled()
        {
            IsInstalled = true;
        }

        public void MarkNotInstalled()
        {
            IsInstalled = false;
        }

        public void MarkEnabled()
        {
            if (!IsInstalled)
                throw new InvalidOperationException("Can only mark enabled when the plugin is already marked installed.");

            IsEnabled = true;
        }

        public void MarkDisabled()
        {
            if (!IsInstalled)
                throw new InvalidOperationException("Can only mark disabled when the plugin is already marked installed.");

            IsEnabled = false;
        }
    }

    static class LoadedPluginEnumerableExtensions
    {
        public static LoadedPlugin Find(this IEnumerable<LoadedPlugin> plugins, string pluginName)
        {
            return plugins.FirstOrDefault(it => it.PluginDefinition.Name == pluginName);
        }

        public static IEnumerable<LoadedPlugin> WhereInstalled(this IEnumerable<LoadedPlugin> plugins)
        {
            return plugins.Where(it => it.IsInstalled);
        }

        public static IEnumerable<LoadedPlugin> WhereNotInstalled(this IEnumerable<LoadedPlugin> plugins)
        {
            return plugins.Where(it => !it.IsInstalled);
        }

        public static IEnumerable<LoadedPlugin> WhereEnabled(this IEnumerable<LoadedPlugin> plugins)
        {
            return plugins.Where(it => it.IsInstalled && it.IsEnabled);
        }

        public static IEnumerable<LoadedPlugin> WhereDisabled(this IEnumerable<LoadedPlugin> plugins)
        {
            return plugins.Where(it => it.IsInstalled && !it.IsEnabled);
        }
    }
}
