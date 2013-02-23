using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Analytics
{
    public class AnalyticsPlugin : PluginBase
    {
        public override void OnStartup(PluginLifecycleContext context)
        {
            PageLifecycleInterceptors.Interceptors.Add(new AnalyticsPageLifecycleInterceptor());
        }

        public override void OnDisable(PluginLifecycleContext context)
        {
            PageLifecycleInterceptors.Interceptors.Remove(typeof(AnalyticsPageLifecycleInterceptor));
        }
    }
}