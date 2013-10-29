using Seeger.Plugins.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.UI;

namespace Seeger.Web.UI
{
    static class WidgetControlLoader
    {
        public static Control Load(WidgetDefinition widget, Page hostingPage, bool loadInDesignMode)
        {
            Require.NotNull(widget, "widget");
            Require.NotNull(hostingPage, "hostingPage");

            Control control = null;

            if (loadInDesignMode)
            {
                if (File.Exists(HostingEnvironment.MapPath(widget.DesignerVirtualPath)))
                {
                    control = hostingPage.LoadControl(widget.DesignerVirtualPath);
                }
                else
                {
                    control = new DefaultWidgetControl();
                }
            }
            else
            {
                control = hostingPage.LoadControl(widget.WidgetControlVirtualPath);
            }

            var widgetControl = control as WidgetControlBase;

            if (widgetControl != null)
            {
                widgetControl.Widget = widget;
                widgetControl.IsInDesignMode = loadInDesignMode;
            }

            return control;
        }
    }
}
