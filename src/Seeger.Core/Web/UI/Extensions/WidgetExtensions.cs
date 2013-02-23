using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web;
using System.Globalization;

using Seeger.Caching;
using Seeger.Plugins.Widgets;
using Seeger.Templates;

namespace Seeger.Web.UI
{
    public static class WidgetExtensions
    {
        #region UI Rendering

        public static WidgetControlBase LoadWidgetControl(this WidgetDefinition widget, Page page)
        {
            Require.NotNull(widget, "widget");
            Require.NotNull(page, "page");

            Control ctrl = page.LoadControl(widget.WidgetControlVirtualPath);
            if (ctrl is WidgetControlBase)
            {
                return ctrl as WidgetControlBase;
            }

            throw new InvalidOperationException(
                String.Format("Cannot find widget control on the page. Ensure that your widget control inherits from {0}. Widget Name: {1}.",
                typeof(WidgetControlBase).FullName,
                widget.Name));
        }

        public static WidgetDesignerBase LoadDesigner(this WidgetDefinition widget, Page page)
        {
            Require.NotNull(widget, "widget");
            Require.NotNull(page, "page");

            WidgetDesignerBase designer = null;

            if (File.Exists(page.Server.MapPath(widget.DesignerVirtualPath)))
            {
                Control ctrl = page.LoadControl(widget.DesignerVirtualPath);
                if (ctrl != null)
                {
                    designer = ctrl as WidgetDesignerBase;
                    if (designer == null)
                    {
                        throw new InvalidOperationException(
                            String.Format("Widget designer control must inherit from {0}. Widget Name: {1}.",
                            typeof(WidgetDesignerBase).FullName,
                            widget.Name));
                    }
                }
            }

            if (designer == null)
            {
                designer = (DefaultWidgetDesigner)page.LoadControl(typeof(DefaultWidgetDesigner), null);
            }

            designer.WidgetName = widget.Name;
            designer.PluginName = widget.Plugin.Name;

            return designer;
        }

        public static void RenderDesigner(this WidgetDefinition widget, TextWriter writer)
        {
            Require.NotNull(widget, "widget");
            Require.NotNull(writer, "writer");

            ControlRenderingHostPage page = new ControlRenderingHostPage();

            WidgetDesignerBase designer = LoadDesigner(widget, page);
            page.Controls.Add(designer);

            HttpContext.Current.Server.Execute(page, writer, true);
        }

        public static void TryAddToPage(this WidgetDefinition widget, LayoutPageBase page, Zone block, WidgetInPage setting)
        {
            Require.NotNull(widget, "widget");
            Require.NotNull(page, "page");
            Require.NotNull(block, "block");

            ZoneControl blockControl = block.LoadZoneControl(page);
            if (blockControl == null)
            {
                return;
            }

            WidgetControlBase widgetControl = LoadWidgetControl(widget, page);
            if (widgetControl != null)
            {
                widgetControl.WidgetInPage = setting;
                blockControl.Controls.Add(widgetControl);
            }
        }

        #endregion
    }
}
