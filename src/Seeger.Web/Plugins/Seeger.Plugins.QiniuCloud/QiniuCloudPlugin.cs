using Seeger.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.QiniuCloud
{
    public class QiniuCloudPlugin : PluginBase
    {
        public override void OnStartup(PluginLifecycleContext context)
        {
            FileSystemProviders.Register(new QiniuFileSystemProvider());
        }

        public override void OnDisable(PluginLifecycleContext context)
        {
            FileSystemProviders.Unregister(Strings.FileSystemProviderName);
        }
    }
}