using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.AppKeepAlive
{
    public class AppKeepAlivePlugin : PluginBase
    {
        public override void OnStartup(PluginLifecycleContext context)
        {
            var settings = GlobalSettingManager.Instance;
            var url = settings.GetValue(SettingKeys.Url);

            if (!String.IsNullOrEmpty(url))
            {
                var interval = settings.TryGetValue<int>(SettingKeys.IntervalInMinutes, Constants.DefaultIntervalInMinutes);
                if (interval == 0)
                {
                    interval = Constants.DefaultIntervalInMinutes;
                }

                KeepAliveWorker.Start(url, TimeSpan.FromMinutes(interval));
            }
        }

        public override void OnDisable(PluginLifecycleContext context)
        {
            KeepAliveWorker.Stop(true);
        }
    }
}