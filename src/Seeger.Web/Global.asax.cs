using Seeger.Config;
using Seeger.Data;
using Seeger.Data.Mapping.Mappers;
using Seeger.Events;
using Seeger.Files;
using Seeger.Licensing;
using Seeger.Logging;
using Seeger.Plugins;
using Seeger.Tasks;
using Seeger.Web.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
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

            // Attribute mapping factory
            var mapperFactory = new AttributeMapperFactory();
            mapperFactory.RegisterMappers(new[] { Assembly.Load("Seeger.Core") });

            AttributeMapperFactories.Current = mapperFactory;

            Database.Initialize();
            PluginManager.StartupEnabledPlugins();
            TaskQueueExecutor.Start();

            ResourceBundler.Initialize();
            WebApiConfig.Configure(System.Web.Http.GlobalConfiguration.Configuration);

            // TODO: Better to provide some IStartupTask interface
            if (FileBucketMetaStores.Current.GetBucketCount() == 0)
            {
                var service = new FileBucketService(FileBucketMetaStores.Current);
                var meta = service.CreateBucket("Local", "Local", new Dictionary<string, string>
                {
                    { "BaseVirtualPath", "/Files/" }
                });

                service.SetDefault(meta.BucketId);
            }

            Event.Raise(new ApplicationStarted(this));
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            Event.Raise(new HttpRequestBegun(this));
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
            Event.Raise(new HttpRequestEnded(this));
        }

        void Application_End(object sender, EventArgs e)
        {
            Event.Raise(new ApplicationEnded(this));
            _logger.Info(UserReference.System(), "Application ended");
        }

        void Application_Error(object sender, EventArgs e)
        {
            if (!VirtualPathUtility.GetFileName(Request.RawUrl).IgnoreCaseEquals("favicon.ico"))
            {
                _logger.ErrorException(UserReference.System(), Server.GetLastError(), Request.RawUrl);
            }

            Event.Raise(new ApplicationErrorOccurred(this));
        }

        void Session_Start(object sender, EventArgs e)
        {
            Event.Raise(new HttpSessionStarted(this));
        }

        void Session_End(object sender, EventArgs e)
        {
            Event.Raise(new HttpSessionEnded(this));
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            var evnt = new VaryByCustomStringRequested(custom, this);

            Event.Raise(evnt);

            if (evnt.Result != null)
            {
                return evnt.Result;
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
}
