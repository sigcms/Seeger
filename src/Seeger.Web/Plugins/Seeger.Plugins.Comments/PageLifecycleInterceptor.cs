using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments
{
    public class PageLifecycleInterceptor : EmptyPageLifecycleInterceptor
    {
        public override void OnLoad(System.Web.UI.Page page)
        {
            base.OnLoad(page);

            var layoutPage = page as LayoutPageBase;

            layoutPage.PageResources.Styles.Add(new StyleResource
            {
                Path = "/Plugins/" + Strings.PluginName + "/Styles/comments.css"
            });
        }
    }
}