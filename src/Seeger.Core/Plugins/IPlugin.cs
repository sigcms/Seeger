using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Plugins
{
    public interface IPlugin
    {
        void OnInstall(PluginLifecycleContext context);

        void OnEnable(PluginLifecycleContext context);

        void OnDisable(PluginLifecycleContext context);

        void OnStartup(PluginLifecycleContext context);

        void OnUninstall(PluginLifecycleContext context);
    }

    public abstract class PluginBase : IPlugin
    {
        public virtual void OnInstall(PluginLifecycleContext context) { }

        public virtual void OnEnable(PluginLifecycleContext context) { }

        public virtual void OnDisable(PluginLifecycleContext context) { }

        public virtual void OnStartup(PluginLifecycleContext context) { }

        public virtual void OnUninstall(PluginLifecycleContext context) { }
    }
}
