using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Web;

namespace Seeger
{
    public static class CmsVirtualPath
    {
        public static string GetFull(string virtualPathRelativeToCmsRoot)
        {
            if (String.IsNullOrEmpty(virtualPathRelativeToCmsRoot))
            {
                return InstallationInfo.InstallPath;
            }

            virtualPathRelativeToCmsRoot = virtualPathRelativeToCmsRoot.TrimStart('~');

            return UrlUtility.Combine(InstallationInfo.InstallPath, virtualPathRelativeToCmsRoot);
        }

        public static string ForPlugin(string pluginName)
        {
            return UrlUtility.Combine(InstallationInfo.InstallPath, "Plugins/" + pluginName);
        }
    }
}
