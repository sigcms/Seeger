using Seeger.Data;
using Seeger.Licensing;
using Seeger.Plugins;
using Seeger.Tasks;
using System.Linq;
using System.Reflection;

namespace Seeger
{
    public static class CmsInitializer
    {
        public static void Initialize()
        {
            CmsConfiguration.Initialize();
            LicensingService.ValidateCurrentLicense();
            NhSessionManager.Initialize();
            PluginManager.StartupEnabledPlugins();
            TaskQueueExecutor.Start();
        }
    }
}
