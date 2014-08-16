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
                layoutPage.Styles.Add(new StyleResource { Path = "/Plugins/" + Strings.PluginName + "/Scripts/jquery.slides.css" });
                layoutPage.FootScripts.Add(new ScriptResource { Path = "/Plugins/" + Strings.PluginName + "/Scripts/jquery.slides.min.js" });
            }
        }
    }
}