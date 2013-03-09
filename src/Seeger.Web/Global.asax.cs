using NLog;
using Seeger.Data;
using System;
using System.Web;

namespace Seeger.Web.UI
{
    public class Global : System.Web.HttpApplication
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        void Application_Start(object sender, EventArgs e)
        {
            _logger.Debug("Application starting...");

            CmsInitializer.Initialize();
            ResourceBundler.Initialize();
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            Database.CloseCurrentSession();
        }

        void Application_End(object sender, EventArgs e)
        {
            _logger.Debug("Application ended.");
        }

        void Application_Error(object sender, EventArgs e)
        {
            if (!VirtualPathUtility.GetFileName(Request.RawUrl).IgnoreCaseEquals("favicon.ico"))
            {
                _logger.ErrorException(Request.RawUrl, Server.GetLastError());
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
        }

        void Session_End(object sender, EventArgs e)
        {
        }
    }
}
