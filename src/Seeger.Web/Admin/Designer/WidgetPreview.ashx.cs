using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Seeger.Plugins;

namespace Seeger.Web.UI.Admin.Designer
{
    // Note: All query strings of the designer page must be passed to this handler, because the widget designer control might use them.
    public class WidgetPreview : IHttpHandler
    {
        private string WidgetName
        {
            get { return HttpContext.Current.Request["widgetName"]; }
        }

        private string PluginName
        {
            get { return HttpContext.Current.Request["pluginName"] ?? String.Empty; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (String.IsNullOrEmpty(WidgetName))
                throw new InvalidOperationException("'Missing query string parameter 'name'.");

            var plugin = PluginManager.FindEnabledPlugin(PluginName);

            if (plugin == null)
                throw new InvalidOperationException("Plugin " + PluginName + " was not found or not enabled.");

            var widget = plugin.FindWidget(WidgetName);

            if (widget == null)
                throw new InvalidOperationException(String.Format("Widget '{0}' was not found.", WidgetName));

            WidgetDesignerRenderer.Render(widget, new HttpContextWrapper(context), context.Response.Output);
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