using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seeger.Plugins
{
    public class DefaultInstalledPluginService : IInstalledPluginService
    {
        private string _filePath;
        private List<InstalledPlugin> _items;

        public DefaultInstalledPluginService(string filePath)
        {
            Require.NotNullOrEmpty(filePath, "filePath");
            _filePath = filePath;
        }

        private void EnsureLoaded()
        {
            if (_items != null) return;

            var items = new List<InstalledPlugin>();

            if (File.Exists(_filePath))
            {
                foreach (var line in File.ReadAllLines(_filePath, Encoding.UTF8))
                {
                    var parts = line.Split(',');
                    var pluginName = parts[0];
                    var enabled = Convert.ToBoolean(parts[1]);

                    items.Add(new InstalledPlugin(pluginName, enabled));
                }
            }

            _items = items;
        }

        public bool Contains(string pluginName)
        {
            return Find(pluginName) != null;
        }

        public InstalledPlugin Find(string pluginName)
        {
            EnsureLoaded();
            return _items.FirstOrDefault(it => it.PluginName == pluginName);
        }

        public IEnumerable<InstalledPlugin> FindAll()
        {
            EnsureLoaded();
            return _items;
        }

        public void Add(string pluginName, bool enabled)
        {
            EnsureLoaded();
            _items.Add(new InstalledPlugin(pluginName, enabled));
        }

        public bool Remove(string pluginName)
        {
            EnsureLoaded();
            var plugin = Find(pluginName);
            return plugin == null ? false : _items.Remove(plugin);
        }

        public void MarkEnabled(string pluginName)
        {
            EnsureLoaded();
            var plugin = Find(pluginName);
            plugin.IsEnabled = true;
        }

        public void MarkDisabled(string pluginName)
        {
            EnsureLoaded();
            var plugin = Find(pluginName);
            plugin.IsEnabled = false;
        }

        public void SaveChanges()
        {
            if (_items == null) return;

            using (var writer = new StreamWriter(_filePath, false, Encoding.UTF8))
            {
                foreach (var item in _items)
                {
                    writer.WriteLine(item.Serialize());
                }

                writer.Flush();
            }
        }
    }
}
