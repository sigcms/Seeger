using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Seeger.Plugins;

namespace Seeger.Web.UI.Admin.Designer
{
    public class WidgetPreview : IHttpHandler
    {
        private string CultureName
        {
            get { return HttpContext.Current.Request["culture"]; }
        }

        private string WidgetName
        {
            get { return HttpContext.Current.Request["widgetName"]; }
        }

        private string TemplateName
        {
            get { return HttpContext.Current.Request["templateName"] ?? String.Empty; }
        }

        private string PluginName
        {
            get { return HttpContext.Current.Request["pluginName"] ?? String.Empty; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            RenderWidgetPreview();
        }

        private void RenderWidgetPreview()
        {
            if (String.IsNullOrEmpty(WidgetName))
            {
                HttpContext.Current.Response.Write("'Missing query string parameter 'name'.");
                return;
            }

            var plugin = PluginManager.FindEnabledPlugin(PluginName);

            if (plugin == null)
                throw new InvalidOperationException("Plugin " + PluginName + " was not found or not enabled.");

            var widget = plugin.FindWidget(WidgetName);

            if (widget == null)
                throw new InvalidOperationException(String.Format("Widget '{0}' was not found.", WidgetName));

            widget.RenderDesigner(HttpContext.Current.Response.Output);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}