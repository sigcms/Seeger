using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Globalization;
using System.Globalization;

namespace Seeger.Web.UI.Admin.Controls
{
    public partial class UpgradeBrowserHint : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 8)
            {
                Hint.Visible = true;

                string hintFormat = null;

                if (Request.Browser.MajorVersion < 7)
                {
                    hintFormat = T("Dashboard.IE6Hint");
                }
                else
                {
                    hintFormat = T("Dashboard.OutOfDateBrowserHint");
                }

                HintMessage.Text = hintFormat.Replace("{Browser}", "IE").Replace("{Version}", Request.Browser.Version);
            }
        }

        protected string T(string key)
        {
            return ResourcesFolder.Global.GetValue(key, CultureInfo.CurrentUICulture);
        }
    }
}