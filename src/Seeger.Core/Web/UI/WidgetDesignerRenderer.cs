using Seeger.Plugins.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.UI
{
    public static class WidgetDesignerRenderer
    {
        public static void Render(WidgetDefinition widget, HttpContextBase context, TextWriter writer)
        {
            Require.NotNull(widget, "widget");
            Require.NotNull(context, "context");
            Require.NotNull(writer, "writer");

            var page = new WidgetRenderingHostPage();
            var control = WidgetControlLoader.Load(widget, page, true);
            page.Controls.Add(control);

            context.Server.Execute(page, writer, true);
        }
    }
}
