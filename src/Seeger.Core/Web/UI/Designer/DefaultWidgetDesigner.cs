using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class DefaultWidgetDesigner : WidgetDesignerBase
    {
        protected override void RenderWidgetContent(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<div class=\"sig-default-widget\"><span class='sig-widget-name'>" + Widget.DisplayName.Localize() + "</span></div>");
        }
    }
}
