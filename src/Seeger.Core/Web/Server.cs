using System;

namespace Seeger.Web
{
    public class Server
    {
        public static string MapPath(string virtualPath)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
        }

        public static string RootPhysicalPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
    }
}
