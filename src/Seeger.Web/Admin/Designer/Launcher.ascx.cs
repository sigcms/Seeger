using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Designer
{
    public partial class Launcher : UserControl
    {
        protected string DesignerUrl
        {
            get
            {
                var layoutPage = (LayoutPageBase)Page;
                var page = layoutPage.PageItem;
                var url = page.Layout.AspxVirtualPath + "?pageid=" + page.Id + "&design=true";

                if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
                {
                    url += "&culture=" + layoutPage.PageCulture.Name;
                }

                return url;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string T(string key)
        {
            return ResourceFolder.Global.GetValue(key, AdminSession.Current.UICulture ?? CultureInfo.CurrentUICulture) ?? key;
        }
    }
}