using Seeger.Config;
using Seeger.Data;
using Seeger.Events;
using Seeger.Licensing;
using Seeger.Logging;
using Seeger.Plugins;
using Seeger.Tasks;
using Seeger.Web.Events;
using System;
using System.Web;

namespace Seeger.Web.UI
{
    public class Global : System.Web.HttpApplication
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        void Application_Start(object sender, EventArgs e)
        {
            _logger.Info(UserReference.System(), "Application starting...");

            CmsConfiguration.Initialize();
            LicensingService.ValidateCurrentLicense();
            Database.Initialize();
            PluginManager.StartupEnabledPlugins();
            TaskQueueExecutor.Start();

            ResourceBundler.Initialize();
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        void Application_PostAuthorizeRequest(object sender, EventArgs e)
        {
            if (Request.RawUrl.StartsWith("/Admin/", StringComparison.OrdinalIgnoreCase))
            {
                var culture = AdminSession.Current.UICulture;
                if (culture != null)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            Database.CloseCurrentSession();
        }

        void Application_End(object sender, EventArgs e)
        {
            _logger.Info(UserReference.System(), "Application ended");
        }

        void Application_Error(object sender, EventArgs e)
        {
            if (!VirtualPathUtility.GetFileName(Request.RawUrl).IgnoreCaseEquals("favicon.ico"))
            {
                _logger.ErrorException(UserReference.System(), Server.GetLastError(), Request.RawUrl);
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
        }

        void Session_End(object sender, EventArgs e)
        {
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            var evnt = new VaryByCustomStringRequested(custom, new HttpContextWrapper(context));

            Event.Raise(evnt);

            if (evnt.Result != null)
            {
                return evnt.Result;
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
}
