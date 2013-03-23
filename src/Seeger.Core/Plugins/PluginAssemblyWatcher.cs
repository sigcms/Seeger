using Seeger.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Seeger.Logging;

namespace Seeger.Plugins
{
    static class PluginAssemblyWatcher
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();
        static readonly Dictionary<string, FileSystemWatcher> _watchers = new Dictionary<string, FileSystemWatcher>();

        public static void Watch(string pluginName)
        {
            lock (_watchers)
            {
                var pluginBinDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins\\" + pluginName + "\\bin");
                var watcher = new FileSystemWatcher(pluginBinDirectory)
                {
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = false,
                    Filter = "*.dll",
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
                };

                watcher.Changed += (sender, e) =>
                {
                    OnPluginAssemblyChanged(pluginName, e);
                };

                _watchers.Add(pluginName, new FileSystemWatcher(pluginBinDirectory));
            }
        }

        public static void Unwatch(string pluginName)
        {
            lock (_watchers)
            {
                _watchers.Remove(pluginName);
            }
        }

        static void OnPluginAssemblyChanged(string pluginName, FileSystemEventArgs e)
        {
            _log.Debug(UserReference.System(), "Plugin assembly changed. Plugin: " + pluginName + ", file: " + e.Name + ", type: " + e.ChangeType);
            Server.TouchRootWebConfig();
        }
    }
}
