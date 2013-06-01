using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    class DefaultWidgetControl : WidgetControlBase
    {
        protected override void RenderChildren(System.Web.UI.HtmlTextWriter writer)
        {
            var culture = PageCulture;

            if (IsInDesignMode)
            {
                culture = AdminSession.Current.UICulture;
            }

            writer.Write("<div class=\"sig-default-widget\"><span class='sig-widget-name'>" + Widget.DisplayName.Localize(culture) + "</span></div>");
        }
    }
}
