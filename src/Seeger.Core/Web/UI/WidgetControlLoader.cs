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
        public static WidgetControlBase Load(WidgetDefinition widget, Page hostingPage, bool loadInDesignMode)
        {
            Require.NotNull(widget, "widget");
            Require.NotNull(hostingPage, "hostingPage");

            WidgetControlBase control = null;

            if (loadInDesignMode)
            {
                if (File.Exists(HostingEnvironment.MapPath(widget.DesignerVirtualPath)))
                {
                    control = hostingPage.LoadControl(widget.DesignerVirtualPath) as WidgetControlBase;
                }
                else
                {
                    control = new DefaultWidgetControl();
                }
            }
            else
            {
                control = hostingPage.LoadControl(widget.WidgetControlVirtualPath) as WidgetControlBase;
            }

            if (control == null)
                throw new InvalidOperationException(
                    String.Format("Cannot not load widget control for widget \"{0}\". Ensure that your widget control inherits from {1}.", typeof(WidgetControlBase).FullName, widget.Name));

            control.Widget = widget;
            control.IsInDesignMode = loadInDesignMode;

            return control;
        }
    }
}
