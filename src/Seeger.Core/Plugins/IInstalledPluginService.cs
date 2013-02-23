using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Seeger.Plugins
{
    public interface IInstalledPluginService
    {
        bool Contains(string pluginName);

        InstalledPlugin Find(string pluginName);

        IEnumerable<InstalledPlugin> FindAll();

        void Add(string pluginName, bool enabled);

        bool Remove(string pluginName);

        void MarkEnabled(string pluginName);

        void MarkDisabled(string pluginName);

        void SaveChanges();
    }

    public static class InstalledPluginServices
    {
        public static Func<IInstalledPluginService> Current = () => new DefaultInstalledPluginService(HostingEnvironment.MapPath("/App_Data/InstalledPlugins.txt"));
    }
}
