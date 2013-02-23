using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seeger.Plugins
{
    public class InstalledPlugins
    {
        private HashSet<string> _plugins = new HashSet<string>();

        public IEnumerable<string> All
        {
            get
            {
                return _plugins;
            }
        }

        public InstalledPlugins()
        {
        }

        public bool Contains(string pluginName)
        {
            return _plugins.Contains(pluginName);
        }

        public void Add(string pluginName)
        {
            _plugins.Add(pluginName);
        }

        public bool Remove(string pluginName)
        {
            return _plugins.Remove(pluginName);
        }

        public static InstalledPlugins LoadFrom(string filePath)
        {
            var service = new InstalledPlugins();

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath, Encoding.UTF8))
                {
                    var pluginName = line.Trim();

                    if (!String.IsNullOrWhiteSpace(pluginName))
                    {
                        service.Add(pluginName);
                    }
                }
            }

            return service;
        }

        public void Save(string path)
        {
            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                Save(writer);
                writer.Flush();
            }
        }

        public void Save(TextWriter writer)
        {
            foreach (var plugin in _plugins.OrderBy(it => it))
            {
                writer.WriteLine(plugin);
            }
        }
    }
}
