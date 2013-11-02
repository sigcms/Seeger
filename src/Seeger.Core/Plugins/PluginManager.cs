using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using Seeger.Plugins.Widgets;
using Seeger.Plugins.Loaders;
using Seeger.Data;
using Seeger.Logging;
using Seeger.Events;

namespace Seeger.Plugins
{
    public static class PluginManager
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        static List<LoadedPlugin> _loadedPlugins = new List<LoadedPlugin>();

        public static bool WasInitialized { get; private set; }

        public static event EventHandler PluginStartedup;

        public static event EventHandler PluginEnabled;

        public static event EventHandler PluginDisabled;

        public static event EventHandler PluginInstalled;

        public static event EventHandler PluginUninstalled;

        public static IEnumerable<PluginDefinition> LoadedPlugins
        {
            get
            {
                return _loadedPlugins.Select(it => it.PluginDefinition);
            }
        }

        public static IEnumerable<PluginDefinition> InstalledPlugins
        {
            get
            {
                return _loadedPlugins.WhereInstalled().Select(it => it.PluginDefinition);
            }
        }

        public static IEnumerable<PluginDefinition> EnabledPlugins
        {
            get
            {
                return _loadedPlugins.WhereInstalled().WhereEnabled().Select(it => it.PluginDefinition);
            }
        }

        public static IEnumerable<PluginDefinition> DisabledPlugins
        {
            get
            {
                return _loadedPlugins.WhereInstalled().WhereDisabled().Select(it => it.PluginDefinition);
            }
        }

        public static bool IsLoaded(string pluginName)
        {
            return LoadedPlugins.Any(it => it.Name == pluginName);
        }

        public static bool IsInstalled(string pluginName)
        {
            return InstalledPlugins.Any(it => it.Name == pluginName);
        }

        public static bool IsEnabled(string pluginName)
        {
            return EnabledPlugins.Any(it => it.Name == pluginName);
        }

        public static PluginDefinition FindLoadedPlugin(string pluginName)
        {
            return LoadedPlugins.FirstOrDefault(it => it.Name == pluginName);
        }

        public static PluginDefinition FindEnabledPlugin(string pluginName)
        {
            return EnabledPlugins.FirstOrDefault(it => it.Name == pluginName);
        }

        public static void StartupEnabledPlugins()
        {
            var enabledPlugins = EnabledPlugins.ToList();

            foreach (var plugin in enabledPlugins)
            {
                Startup(plugin, false);
            }

            if (enabledPlugins.Any(x => NhMappingProviders.HasMappingProvider(x.Name)))
            {
                Database.Initialize();
            }
        }

        public static void Startup(PluginDefinition plugin)
        {
            Startup(plugin, true);
        }

        static void Startup(PluginDefinition plugin, bool reinitNhSessionManager)
        {
            Require.NotNull(plugin, "plugin");

            var loadedPlugin = _loadedPlugins.Find(plugin.Name);

            EventEnvironment.HandlerRegistry.RegisterHandlers(plugin.Name, loadedPlugin.Assemblies);

            if (plugin.PluginType != null)
            {
                var pluginImpl = (IPlugin)Activator.CreateInstance(plugin.PluginType);
                pluginImpl.OnStartup(new PluginLifecycleContext(plugin));
            }

            if (reinitNhSessionManager && NhMappingProviders.HasMappingProvider(plugin.Name))
            {
                Database.Initialize();
            }

            if (PluginStartedup != null)
                PluginStartedup(null, EventArgs.Empty);
        }

        public static void Enable(string pluginName)
        {
            Require.NotNullOrEmpty(pluginName, "pluginName");
            Require.That(IsLoaded(pluginName), "Cannot enable not loaded plugins. Plugin name: " + pluginName);
            Require.That(IsInstalled(pluginName), "Cannot enable not installed plugins. Plugin name: " + pluginName);

            var plugin = _loadedPlugins.Find(pluginName);

            if (!plugin.IsEnabled)
            {
                if (plugin.PluginDefinition.PluginType != null)
                {
                    var pluginImpl = (IPlugin)Activator.CreateInstance(plugin.PluginDefinition.PluginType);
                    pluginImpl.OnEnable(new PluginLifecycleContext(plugin.PluginDefinition));
                }

                var service = InstalledPluginServices.Current();
                service.MarkEnabled(pluginName);
                service.SaveChanges();

                plugin.MarkEnabled();

                if (PluginEnabled != null)
                    PluginEnabled(null, EventArgs.Empty);

                Startup(plugin.PluginDefinition);
            }
        }

        public static void Disable(string pluginName)
        {
            Require.NotNullOrEmpty(pluginName, "pluginName");
            Require.That(IsLoaded(pluginName), "Cannot disable not loaded plugins. Plugin name: " + pluginName);
            Require.That(IsInstalled(pluginName), "Cannot disable not installed plugins. Plugin name: " + pluginName);

            var plugin = _loadedPlugins.Find(pluginName);

            if (plugin.IsEnabled)
            {
                if (plugin.PluginDefinition.PluginType != null)
                {
                    var pluginImpl = (IPlugin)Activator.CreateInstance(plugin.PluginDefinition.PluginType);
                    pluginImpl.OnDisable(new PluginLifecycleContext(plugin.PluginDefinition));
                }

                var service = InstalledPluginServices.Current();
                service.MarkDisabled(pluginName);
                service.SaveChanges();

                EventEnvironment.HandlerRegistry.RemoveHandlers(plugin.PluginDefinition.Name);

                plugin.MarkDisabled();

                if (PluginDisabled != null)
                    PluginDisabled(null, EventArgs.Empty);
            }
        }

        public static void Install(string pluginName)
        {
            Require.NotNullOrEmpty(pluginName, "pluginName");

            var plugin = InstalledPlugins.FindByName(pluginName);

            if (plugin != null)
                throw new InvalidOperationException("Plugin \"" + plugin.DisplayName + "\" has already been installed.");

            plugin = LoadedPlugins.FindByName(pluginName);

            if (plugin == null)
                throw new InvalidOperationException("Plugin \"" + pluginName + "\" is not loaded.");

            _log.Debug(UserReference.System(), "Installing plugin \"" + pluginName + "\"");

            if (plugin.PluginType != null)
            {
                var pluginImpl = (IPlugin)Activator.CreateInstance(plugin.PluginType);
                pluginImpl.OnInstall(new PluginLifecycleContext(plugin));
            }

            MarkInstalled(plugin);

            if (PluginInstalled != null)
                PluginInstalled(null, EventArgs.Empty);
        }

        public static void InstallAndEnable(string pluginName)
        {
            Install(pluginName);
            Enable(pluginName);
        }

        public static void Uninstall(string pluginName)
        {
            Require.NotNullOrEmpty(pluginName, "pluginName");

            var plugin = _loadedPlugins.Find(pluginName);

            if (plugin == null || !plugin.IsInstalled)
                throw new InvalidOperationException("Plugin \"" + plugin.PluginDefinition.DisplayName + "\" is not installed.");

            if (plugin.IsEnabled)
            {
                Disable(pluginName);
            }

            if (plugin.PluginDefinition.PluginType != null)
            {
                var pluginImpl = (IPlugin)Activator.CreateInstance(plugin.PluginDefinition.PluginType);
                pluginImpl.OnUninstall(new PluginLifecycleContext(plugin.PluginDefinition));
            }

            MarkUninstalled(plugin.PluginDefinition);

            if (PluginUninstalled != null)
                PluginUninstalled(null, EventArgs.Empty);
        }

        static void MarkInstalled(PluginDefinition plugin)
        {
            var service = InstalledPluginServices.Current();

            if (!service.Contains(plugin.Name))
            {
                service.Add(plugin.Name, false);
                service.SaveChanges();

                _loadedPlugins.Find(plugin.Name).MarkInstalled();
            }
        }

        static void MarkUninstalled(PluginDefinition plugin)
        {
            var service = InstalledPluginServices.Current();

            if (service.Contains(plugin.Name))
            {
                service.Remove(plugin.Name);
                service.SaveChanges();

                _loadedPlugins.Find(plugin.Name).MarkNotInstalled();
            }
        }

        /// <summary>
        /// Initialize the plugin manager. 
        /// This method will be invoked at PreApplicationStart stage in the main web application. 
        /// It's not intended to be used from your code.
        /// </summary>
        public static void Initialize()
        {
            if (WasInitialized)
                throw new InvalidOperationException("PluginManager already intialized.");

            try
            {
                AssemblyDeployer.ClearAssemblies(PluginAssemblyDeployPath);
            }
            catch (Exception ex)
            {
                _log.ErrorException(UserReference.System(), ex, "Clear existing assemblies in probing folder failed.");
            }

            var allPlugins = new List<LoadedPlugin>();

            var loader = PluginLoaders.Current();

            try
            {
                foreach (var pluginFolder in Directory.GetDirectories(HostingEnvironment.MapPath(PluginPaths.ContainingDirectoryVirtualPath)))
                {
                    var pluginName = Path.GetFileName(pluginFolder);

                    try
                    {
                        var assemblies = DeployPluginAssemblies(pluginName);
                        var plugin = loader.Load(pluginName, assemblies);
                        allPlugins.Add(new LoadedPlugin(plugin, assemblies));
                    }
                    catch (Exception ex)
                    {
                        _log.ErrorException(UserReference.System(), ex, "Fail loading plugin: " + pluginName);
                    }
                }
            }
            catch (Exception ex2)
            {
                _log.ErrorException(UserReference.System(), ex2, "Deploy plugin assembly fails.");
            }

            var service = InstalledPluginServices.Current();

            foreach (var each in service.FindAll())
            {
                var plugin = allPlugins.Find(each.PluginName);

                if (plugin == null) continue;

                plugin.MarkInstalled();

                if (each.IsEnabled) plugin.MarkEnabled();
            }

            _loadedPlugins = allPlugins;

            // watch plugin assemblies changes
            foreach (var plugin in _loadedPlugins)
            {
                PluginAssemblyWatcher.Watch(plugin.PluginDefinition.Name);
            }

            WasInitialized = true;
        }

        static readonly string PluginAssemblyDeployPath = HostingEnvironment.MapPath("/App_Data/Assemblies/Plugins");

        static IEnumerable<Assembly> DeployPluginAssemblies(string pluginName)
        {
            _log.Debug(UserReference.System(), "Deploying assemblies for plugin " + pluginName);

            var pluginFolderPath = HostingEnvironment.MapPath(PluginPaths.PluginDirectoryVirtualPath(pluginName));
            var pluginBinFolderPath = Path.Combine(pluginFolderPath, "bin");
            var targetBinFolderPath = Path.Combine(PluginAssemblyDeployPath, pluginName);

            return AssemblyDeployer.DeployAssemblies(pluginBinFolderPath, targetBinFolderPath);
        }

    }
}
