using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Seeger.Plugins.ImageSlider
{
    public class PageLifecycleInterceptor : EmptyPageLifecycleInterceptor
    {
        public override void OnLoad(System.Web.UI.Page page)
        {
            var layoutPage = page as LayoutPageBase;

            if (layoutPage == null) return;

            var pageItem = layoutPage.PageItem;

            if (pageItem.LocatedWidgets.Any(x => x.PluginName == Strings.PluginName && x.WidgetName == "ImageSlider"))
            {
                page.IncludeCss("/Plugins/" + Strings.PluginName + "/Scripts/jquery.slides.css");
                page.Form.Controls.Add(new LiteralControl
                {
                    Text = "<script type=\"text/javascript\" src=\"/Plugins/" + Strings.PluginName + "/Scripts/jquery.slides.min.js\"></script>"
                });
            }
        }
    }
}