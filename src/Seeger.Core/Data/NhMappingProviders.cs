using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data
{
    public static class NhMappingProviders
    {
        static readonly Dictionary<string, Type> _pluginNameProviderTypeMap = new Dictionary<string, Type>();

        public static void Register(string pluginName, Type mappingPoviderType)
        {
            lock (_pluginNameProviderTypeMap)
            {
                if (_pluginNameProviderTypeMap.ContainsKey(pluginName))
                    throw new InvalidOperationException("Cannot duplicate register " + typeof(INhMappingProvider) + ". Plugin name: " + pluginName);

                _pluginNameProviderTypeMap.Add(pluginName, mappingPoviderType);
            }
        }

        public static Type GetMappingProviderType(string pluginName)
        {
            Type type = null;
            if (_pluginNameProviderTypeMap.TryGetValue(pluginName, out type))
            {
                return type;
            }

            return null;
        }

        public static bool HasMappingProvider(string pluginName)
        {
            return _pluginNameProviderTypeMap.ContainsKey(pluginName);
        }
    }
}
