using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Seeger
{
    public class InstallationInfo
    {
        public static readonly string InstallPath;

        static InstallationInfo()
        {
            InstallPath = GetInstallPath();
        }

        private static string GetInstallPath()
        {
            var path = ConfigurationManager.AppSettings["seeger:install-path"];

            if (String.IsNullOrEmpty(path))
            {
                path = "/";
            }

            if (path != "/")
            {
                if (!path.StartsWith("/"))
                {
                    path = "/" + path;
                }
                if (path.EndsWith("/"))
                {
                    path = path.Substring(0, path.Length - 1);
                }
            }

            return path;
        }
    }
}
