using System;
using System.IO;

namespace Seeger.Web
{
    public static class Server
    {
        public static string MapPath(string virtualPath)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
        }

        public static string RootPhysicalPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static void TouchRootWebConfig()
        {
            var path = Path.Combine(RootPhysicalPath, "web.config");
            File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
        }
    }
}
