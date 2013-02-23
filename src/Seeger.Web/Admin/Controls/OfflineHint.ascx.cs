using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Globalization;

namespace Seeger.Web.UI.Admin.Controls
{
    public partial class OfflineHint : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GlobalSettingManager.Instance.FrontendSettings.IsWebsiteOffline)
                {
                    Hint.Visible = true;
                    Hint.Text = ResourcesFolder.Global.GetValue("Message.OfflineHintFormat", CultureInfo.CurrentUICulture);
                }
            }
        }
    }
}