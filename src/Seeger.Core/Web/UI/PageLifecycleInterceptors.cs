using Seeger.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public static class PageLifecycleInterceptors
    {
        static readonly Dictionary<string, List<IPageLifecycleInterceptor>> _pluginNameInterceptorsMap = new Dictionary<string, List<IPageLifecycleInterceptor>>();

        public static void Register(string pluginName, IEnumerable<IPageLifecycleInterceptor> interceptors)
        {
            lock (_pluginNameInterceptorsMap)
            {
                if (!_pluginNameInterceptorsMap.ContainsKey(pluginName))
                {
                    _pluginNameInterceptorsMap.Add(pluginName, new List<IPageLifecycleInterceptor>());
                }

                var list = _pluginNameInterceptorsMap[pluginName];
                list.AddRange(interceptors);
            }
        }

        public static IEnumerable<IPageLifecycleInterceptor> GetInterceptors(string pluginName)
        {
            if (_pluginNameInterceptorsMap.ContainsKey(pluginName))
            {
                return _pluginNameInterceptorsMap[pluginName];
            }

            return new List<IPageLifecycleInterceptor>();
        }

        public static IEnumerable<IPageLifecycleInterceptor> GetEnabledInterceptors()
        {
            var interceptors = new List<IPageLifecycleInterceptor>();
            foreach (var plugin in PluginManager.EnabledPlugins)
            {
                interceptors.AddRange(GetInterceptors(plugin.Name));
            }

            return interceptors;
        }
    }
}
